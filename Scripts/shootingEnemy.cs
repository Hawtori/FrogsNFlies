using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class shootingEnemy : MonoBehaviour
{
    //idle
    //wait some duration
    //shoot at player
    //reposition
    //wait some duration (while repositioning)
    //loop

    enum State
    {
        idle,
        shootAtPlayer,
        reposition
    }
    public int facingDir;
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    public GameObject bullet;
    public float maxSpeed;
    public float speed;
    public float waitTime; // how long before next action
    public float damage; // player touches enemey 
    public float moveTime; // how long enemy can move for
    public float boxLength;

    private Vector3 pos;
    private float xMin = 0, xMax = 0, yMin = 0, yMax = 0;
    private float moveTimer;
    private int currentState = (int)State.shootAtPlayer;
    private bool shootPlayerEh = false;
    private bool repositionEh = false;
    private bool repositioningEh = false;

    public bool idle;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        StartCoroutine(waitToMove(currentState));
    }

    private void Update()
    {
        if (player.transform.position.x > transform.position.x) facingDir = 1;
        else facingDir = -1;

        if (rb.velocity.magnitude < 0.2f) idle = true;
        else idle = false;

        anim.SetInteger("direction", facingDir);


        //create a box where enemy can move inside of
        createBox();

        //if idle state do nothing
        if(currentState == (int)State.idle)
        {
            rb.velocity = Vector2.zero;
            currentState = (int)State.shootAtPlayer;
            StopCoroutine(waitToMove(currentState));
            StartCoroutine(waitToMove(currentState));
        }

        //fire a shot at player
        if (shootPlayerEh)
        {
            anim.SetBool("Shooting", true);
            shoot();
        }

        //reposition then back to idle
        if (repositionEh)
        {
            reposition();
        }

        if(anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }
    }

    void shoot()
    {
        if(gameObject.GetComponent<SpriteRenderer>().sprite.name == "Ranged_3" || gameObject.GetComponent<SpriteRenderer>().sprite.name == "Ranged_9")
        {
            //instantiate the bullet
            Instantiate(bullet, transform.position, Quaternion.identity);
            shootPlayerEh = false;
            currentState = (int)State.reposition;
            {
                anim.SetBool("Shooting", false);
                moveTimer = 0;
                repositionEh = true;
                repositioningEh = false;
            }
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
        //random y between box's y min and max
        if (!repositioningEh)
        {
            pos = new Vector3(getRand(xMin, xMax), getRand(yMin, yMax), -1);
            repositioningEh = true;
        }

        //move towards position
        Vector3 target = pos - transform.position;
        target.Normalize();
        rb.AddForce(target * speed);

        moveTimer += Time.fixedDeltaTime;

        if ((transform.position.x > pos.x - 1f && transform.position.x < pos.x + 1f &&
            transform.position.y > pos.y - 1f && transform.position.y < pos.y + 1f) || moveTimer > moveTime)
        {
            //move to idle state
            currentState = (int)State.idle;
            shootPlayerEh = false;
            repositionEh = false;
            StopCoroutine(waitToMove(currentState));
        }

    }

    void createBox()
    {
        //where the enemy can move inside
        //box center will be directly above the player

        xMin = player.transform.position.x - (boxLength / 2f);
        xMax = xMin + boxLength;

        yMin = player.transform.position.y + 5f;
        yMax = yMin + boxLength;
    }

    IEnumerator waitToMove(int action)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (action == (int)State.shootAtPlayer) shootPlayerEh = true;
        else anim.SetBool("Shooting", false);
        if (action == (int)State.reposition)
        {
            moveTimer = 0;
            repositionEh = true;
            repositioningEh = false;
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
