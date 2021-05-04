using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemy : MonoBehaviour
{
    //idle
    //wait some duration
    //fly towards player
    //hit player and fly away
    //loop

    enum State
    {
        idle,
        flyToPlayer,
        flyAway
    }

    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    public float maxSpeed;
    public float speed;
    public float waitTime; //how long to wait before next action
    public float damage;

    private int currentState = (int)State.idle;
    private bool moveToPlayerEh = false;
    private bool moveAwayPlayerEh = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        //idle state, do nothing
        if (currentState == (int)State.idle)
        {
            rb.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                anim.SetBool("Left", false);
            }
            else
            {
                anim.SetBool("Left", true);
            }
            StartCoroutine(waitToMove((int)State.flyToPlayer)); //move towards player after some duration
            currentState = (int)State.flyToPlayer;
        }

        if (anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }

    }

    private void FixedUpdate()
    {
        //move to player
        if (moveToPlayerEh)
        {
            moveTowardsPlayer();
        }

        //move away from player
        if (moveAwayPlayerEh)
        {
            moveAwayFromPlayer();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveToPlayerEh = false;
            moveAwayPlayerEh = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //deal damage if hit player
            collision.gameObject.GetComponent<Animator>().SetBool("damaged", true);
            GameObject.Find("Player").GetComponent<playerController>().health -= damage;
            StartCoroutine(waitToMove((int)State.idle));
        }
    }

    void moveTowardsPlayer()
    {
        Vector2 target = player.transform.position - transform.position;
        target.Normalize();
        rb.velocity = maxSpeed * target;
        //if (rb.velocity.magnitude < maxSpeed)
        //    rb.AddForce(target * speed);
        if(player.transform.position.x > transform.position.x)
        {
            anim.SetBool("Left", false);
        }
        else
        {
            anim.SetBool("Left", true);
        }
    }

    void moveAwayFromPlayer()
    {
        Vector2 dir = Vector2.zero;
        dir.x = (player.transform.position.x > transform.position.x ? -1 : 1);
        dir.y = 1;
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddForce(dir * speed);

        if (player.transform.position.x < transform.position.x)
        {
            anim.SetBool("Left", false);
        }
        else
        {
            anim.SetBool("Left", true);
        }
    }

    IEnumerator waitToMove(int action)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (action == (int)State.flyToPlayer) moveToPlayerEh = true;
        if (action == (int)State.flyAway) moveAwayPlayerEh = true;
        if (action == (int)State.idle)
        {
            moveAwayPlayerEh = false;
            moveToPlayerEh = false;
            currentState = (int)State.idle;
        }
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
