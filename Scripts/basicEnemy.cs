using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemy : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    public float speed;
    public float maxSpeed;
    public float damage;
    public float distanceToAttack;
    public float knockback;
    public float attackWaitTime;

    private Vector2 movement;
    private float attackWait = 0;
    private bool attackTimeStartEh = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float dist = (transform.position - player.transform.position).magnitude;
        if (dist < distanceToAttack) attackPlayer();
        else moveToPlayer();
        if (attackWait > 0 && attackTimeStartEh) attackWait -= Time.fixedDeltaTime;

        if(player.transform.position.x > transform.position.x)
        {
            anim.SetBool("Left", false);
        }
        else
        {
            anim.SetBool("Left", true);
        }

        if (anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }

    }

    void attackPlayer()
    {
        if(attackWait <= 0)
        {
            StartCoroutine(dealDamage());
            attackWait = attackWaitTime;
            attackTimeStartEh = false;
        }
    }

    void moveToPlayer()
    {
        movement = player.transform.position - transform.position;
        movement.Normalize();
        rb.velocity = movement * maxSpeed;
        //if(rb.velocity.magnitude < maxSpeed)
        //    rb.AddForce(movement * speed * Time.fixedDeltaTime);
    }

    IEnumerator dealDamage()
    {
        yield return new WaitForSecondsRealtime(1f); //see if player still close after 1 sec
        
        Vector2 origin = transform.position;
        Vector2 direction = player.transform.position - transform.position;
        direction.y = 0; //only raycast horizontally, can't attack up
        float distance = distanceToAttack * 0.6f;
        LayerMask mask = LayerMask.GetMask("Player");

        RaycastHit2D ray = Physics2D.Raycast(origin, direction, distance, mask);
        if(ray.collider != null)
        {
            Debug.Log("hit player!");
            Vector2 knockbackDir = new Vector2(direction.x, direction.y + 5);
            knockbackDir.Normalize();
            player.GetComponent<Rigidbody2D>().AddForce(knockbackDir * knockback);
            player.GetComponent<Animator>().SetBool("damaged", true);
            player.GetComponent<playerController>().health -= damage;
        }
        attackTimeStartEh = true;
    }

    IEnumerator resetAnim()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (anim.GetBool("damaged"))
        {
            anim.SetBool("damaged", false);
        }
    }
}
