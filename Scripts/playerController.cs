using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{


    public GameObject animationGO;
    public GameObject animationJC;
    public Animator anim;
    public bool idle;

    public const float jumpTime = 1f;
    public ParticleSystem trial;

    //public GameObject hbar;
    public Rigidbody2D rb;
    public List<GameObject> guns = new List<GameObject>();
    public float maxSpeed = 5f;
    public float gravity = -6f;
    public float maxJump;
    public float health = 100;
    public int facingDir = 1;
    public int currentGun;

    private float maxHealth = 100;
    private float speed = 10f;
    private float jumpForce;
    private float xMove;
    private float jumpStrength;
    private bool jump;
    private bool canJumpEh; // should be: canJumpEh
    private bool jumpingEh;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        canJumpEh = false;
        currentGun = -1;
    }

    private void Update()
    {
        anim.SetInteger("Direction", facingDir);
        
        //inputs and non physcis stuff here
        xMove = 0;

        //only move if not reached max speed yet
        //if max speed, dont take input and just continue with the velocity 
        if(rb.velocity.magnitude < maxSpeed) 
        {
            xMove = Input.GetAxisRaw("Horizontal");
            if (xMove != 0)
            {
                facingDir = (int)xMove;
            }
        }

        //calculate facing direction:
        //if(transform.localScale.x < 0) facingDir = -1;
        //else if(transform.localScale.x > 0) facingDir = 1;

        //has to be grounded to jump
        if(Input.GetKeyDown(KeyCode.Space) && canJumpEh)
        {
            jumpingEh = true;
            jumpStrength = 0.5f;
        }

        //hold space for a bigger jump, up to a max
        if (Input.GetKey(KeyCode.Space) && jumpingEh)
        {
            if (jumpStrength < jumpTime)
            {
                jumpStrength += Time.deltaTime;
            }
            else
            {
                jumpingEh = false;
                Vector3 aayush = transform.position;
                aayush.y += 1.5f;
                GameObject ali = Instantiate(animationJC, aayush, Quaternion.identity);     //nice one dood
                ali.transform.parent = gameObject.transform;
                //Instantiate(trial, transform.position, Quaternion.identity);
            }
        }
    
        //let go to actually jump
        if (Input.GetKeyUp(KeyCode.Space) && canJumpEh)
        {
            SoundManager.PlaySound("jump");
            jumpForce = jumpStrength * maxJump;
            Debug.Log(jumpForce);
            Debug.Log(canJumpEh);               // DON'T DELETE THIS LINE. IT WON'T WORK ON MY PC OTHERWISE.
            jumpingEh = false; 
            jump = true;
            canJumpEh = false;
        }

        //press F to switch gun
        if (Input.GetKeyDown(KeyCode.F) && currentGun != -1)
        {
            if(guns.Count > 1)
            {
                if(currentGun != guns.Count - 1)
                {
                    guns[currentGun].SetActive(false); 
                    guns[++currentGun].SetActive(true);
                }
                else
                {
                    guns[currentGun].SetActive(false);
                    currentGun = 0;
                    guns[currentGun].SetActive(true);

                }
            }
        }

        if(rb.velocity.magnitude > 0.2f)
        {
            anim.SetBool("Idle", false);
        }
        else
        {
            anim.SetBool("Idle", true);
        }

        if(health <= 0)
        {
            Debug.Log("you is funeral");
            Instantiate(animationGO, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GameObject.Find("info").GetComponent<Text>().text = "YOU LOST!";
            SceneManager.LoadScene("EndGame");
        }

        UpdateHealth();

        if (anim.GetBool("damaged"))
            StartCoroutine(resetAnim());
        else
        {
            StopCoroutine(resetAnim());
        }
    }
    IEnumerator die()
    {
        SoundManager.PlaySound("death");
        Instantiate(animationGO, transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.2f);
        reallyDie();
    }
    private void reallyDie() {
        GameObject.Find("info").GetComponent<Text>().text = "YOU LOST!";
        SceneManager.LoadScene("GameOver");
    }

    private void FixedUpdate()
    {
        //do physics update here
        rb.AddForce(new Vector2(0, gravity));
        if (/*groundedEh && */rb.velocity.magnitude < maxSpeed)
        {
            movePlayer();
        }
    }

    void movePlayer()
    {
        //moving player
        Vector2 movement = new Vector2(0, 0);
        if (jump)
        {
            movement.y += jumpForce;
            if(jumpForce >= maxJump * jumpTime)
                movement.x += xMove * speed * 2;
            jump = false;
        }
        movement.x += xMove * speed;
        //if (xMove != 0) movement.y += 100;
        rb.AddForce(movement);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Ground")
        {
            canJumpEh = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            //canJumpEh = false;
            //StartCoroutine(jumpable());
        }
    }

    IEnumerator jumpable()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        canJumpEh = false;
    }

    private void UpdateHealth()
    {
        // calculate health after damage. then call this function.
        float ratio = health / maxHealth;
        if(ratio < 0) ratio = 0;
        GameObject healthbar = GameObject.Find("healthbar");
        healthbar.transform.localScale = new Vector3(ratio, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
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
