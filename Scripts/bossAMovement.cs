using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAMovement : MonoBehaviour
{
    //health > 30%
    //    idle
    //    attack
    //    reposition
    //else
    //    enraged attack
    //    enraged move

    enum State
    {
        idle,
        attack,
        reposition,
        eAttack,        //e = enraged
        eMove
    }

    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    public GameObject bullet;
    public enemyHealth _enemyHealth;


    //enraged///////////////
    public float eMaxSpeed;
    public float eSpeed;
    public float eWaitTime;
    public float eDamage; // player touches enemey 
    //public float eMoveTime; // how long enemy can move for
    ////////////////////////

    public float maxSpeed;
    public float speed;
    public float waitTime; // how long before next action
    public float damage; // player touches enemey 
    public float moveTime; // how long enemy can move for
    public float boxLength;

    private Vector3 pos;
    private float xMin = 0, xMax = 0;
    private float moveTimer;
    private int currentState = (int)State.idle;
    private bool shootPlayerEh = false;
    private bool repositionEh = false;
    private bool repositioningEh = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _enemyHealth = GetComponent<enemyHealth>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (player.transform.position.x > transform.position.x) anim.SetBool("Left", false);
        else anim.SetBool("Left", true);


        //create a box where enemy can move inside of
        createBox();
        //Debug.Log("player position: " + player.transform.position.x +
        //"\nmin: " + xMin + " max: " + xMax);

        //if idle state do nothing
        if(currentState == (int)State.idle)
        {
            rb.velocity = Vector2.zero;
            currentState = (int)State.attack;
            if(_enemyHealth.health < 18) currentState = (int)State.eAttack;
            StopAllCoroutines();
            StartCoroutine(waitToMove(currentState));
            
        }

        //fire a shot at player
        if (shootPlayerEh)
        {
            anim.SetBool("Shooting", true);
            repositionEh = false;
            shoot();
        }

        //reposition then back to idle
        if (repositionEh)
        {
            reposition();
        }

        if (anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<playerController>().health -= damage;
        }
    }

    void shoot()
    {
        if(gameObject.GetComponent<SpriteRenderer>().sprite.name == "Ranged_3" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "Ranged_9")
        {
            //instantiate the bullet
            rb.velocity = Vector2.zero;
            Instantiate(bullet, transform.position, Quaternion.identity);
            shootPlayerEh = false;
            currentState = (int)State.reposition;
            anim.SetBool("Shooting", false);
            StopAllCoroutines();
            StartCoroutine(waitToMove(currentState));
        }
    }

    float getRand(float min, float max)
    {
        return Random.Range(min, max);
    }

    void reposition()
    {
        //move to a random position within the box
        //random x between box's x min and max
        //y stays the same.
        if (!repositioningEh)
        {
            pos = new Vector3(getRand(xMin, xMax), transform.position.y, -1);
            repositioningEh = true;
        }
        //move towards position
        Vector3 target = pos - transform.position;
        target.Normalize();

        if (_enemyHealth.health < 18) rb.velocity = target * eSpeed;     //use enraged speed
        else rb.velocity = target * speed;

        moveTimer += Time.fixedDeltaTime;

        if (((transform.position.x > pos.x - 1f) && (transform.position.x < pos.x + 1f)) || moveTimer > moveTime)
        {
 
            //move to idle state
            currentState = (int)State.idle;
            shootPlayerEh = false;
            repositionEh = false;
            StopAllCoroutines();
        }

    }

    void createBox()
    {
        //where the enemy can move inside
        //box center will be infront of the player

        xMin = player.transform.position.x + 8;
        xMax = xMin + 18;
        //y coord will stay same since manz are on ground
    }

    

    IEnumerator waitToMove(int action)
    {
        float x = waitTime;
        if(_enemyHealth.health < 18) x = waitTime / 2f;            //if enraged, do it 2x as fast.
        yield return new WaitForSecondsRealtime(x);
        if (action == (int)State.attack) shootPlayerEh = true;
        if (action == (int)State.eAttack) shootPlayerEh = true;
        if (action == (int)State.reposition)
        {
            moveTimer = 0;
            repositionEh = true;
            repositioningEh = false;
        }
        if(action != (int)State.attack || action != (int)State.eAttack)
        {
            anim.SetBool("Shooting", false);
        }
    }

    IEnumerator resetAnim()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (anim.GetBool("damaged"))
        {
            anim.SetBool("damaged", false);
        }
    }
}
