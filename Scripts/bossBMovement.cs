using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class bossBMovement : MonoBehaviour
{
    // boss flies around
    // attakcs player and flies away

    // idle
    // wait some duration
    // fly towards player
    // hit player fly away

    // [ ENRAGED ]
    // idle
    // wait less duration
    // fly towards player faster
    // hit player deal more damage

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
    public float waitTime;
    public float damage;

    // enraged mode variables
    private bool enragedEh = false;
    private float enragedMaxSpeed;
    private float enragedSpeed;
    private float enragedWaitTime;
    private float enragedDamage;

    // normal variables
    private float maxHealth;
    private int currentState = (int)State.idle;
    private bool moveToPlayerEh = false;
    private bool moveAwayPlayerEh = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        maxHealth = gameObject.GetComponent<enemyHealth>().health;
        enragedMaxSpeed = maxSpeed * 1.5f;
        enragedSpeed = speed * 1.5f;
        enragedWaitTime = waitTime * 0.75f;
        enragedDamage = damage * 1.5f;
    }

    private void Update()
    {
        if(currentState == (int)State.idle)
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

            currentState = (int)State.flyToPlayer;
            StartCoroutine(waitToMove(currentState));
        }

        if (anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }

        if (gameObject.GetComponent<enemyHealth>().health < (maxHealth * 0.3))
        {
            enragedEh = true;
        }

        if(gameObject.GetComponent<enemyHealth>().health <= 0)
        {
            Debug.Log("sap");
            Destroy(gameObject);
            //GameObject.Find("info").GetComponent<Text>().text = "YOU WON!";
            SceneManager.LoadScene("EndGame");
        }
    }

    private void FixedUpdate()
    {
        if (moveToPlayerEh) moveToPlayer();
        if (moveAwayPlayerEh) moveAwayFromPlayer(); 
    }

    void moveToPlayer()
    {
        Vector2 target = player.transform.position - transform.position;
        target.Normalize();
        if (enragedEh)
        {
            rb.velocity = target * enragedMaxSpeed;
            //if (rb.velocity.magnitude < enragedMaxSpeed)
            //{
            //    rb.AddForce(target * enragedSpeed);
            //}
        }
        else
        {
            rb.velocity = target * maxSpeed;
            //if (rb.velocity.magnitude < maxSpeed)
            //{
            //    rb.AddForce(target * speed);
            //}
        }

        if (player.transform.position.x > transform.position.x)
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
        if (enragedEh)
        {
            Vector2 dir = Vector2.zero;
            dir.x = (player.transform.position.x > transform.position.x ? -1 : 1);
            dir.y = 1;
            rb.velocity = dir * enragedMaxSpeed;
            //if (rb.velocity.magnitude < enragedMaxSpeed)
            //    rb.AddForce(dir * enragedSpeed);
        }
        else
        {
            Vector2 dir = Vector2.zero;
            dir.x = (player.transform.position.x > transform.position.x ? -1 : 1);
            dir.y = 1;
            rb.velocity = dir * maxSpeed;
            //if (rb.velocity.magnitude < maxSpeed)
            //    rb.AddForce(dir * speed);
        }

        if (player.transform.position.x < transform.position.x)
        {
            anim.SetBool("Left", false);
        }
        else
        {
            anim.SetBool("Left", true);
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
            if (enragedEh) GameObject.Find("Player").GetComponent<playerController>().health -= enragedDamage;
            else GameObject.Find("Player").GetComponent<playerController>().health -= damage;
            currentState = (int)State.idle;
            StartCoroutine(waitToMove(currentState));
        }
    }

    IEnumerator waitToMove(int action)
    {
        if(enragedEh) yield return new WaitForSecondsRealtime(enragedWaitTime);
        else yield return new WaitForSecondsRealtime(waitTime);
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
