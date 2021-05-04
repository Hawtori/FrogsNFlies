using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletEnemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public float damage;

    private float aliveTime = 0f;
    private float maxAliveTime = 7f;
    private float bulletForce = 40;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();

        {
            Vector3 adjstZ = transform.position;
            adjstZ.z = 0;
            transform.position = adjstZ;
        }

        rb.AddForce(bulletForce * direction, ForceMode2D.Impulse);
    }

    private void Update()
    {
        aliveTime += Time.fixedDeltaTime;
        if (aliveTime > maxAliveTime) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<playerController>() != null)
        {
            collision.gameObject.GetComponent<playerController>().health -= damage;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<playerController>() != null)
        {
            if(collision.gameObject.GetComponent<Animator>() != null)
            collision.gameObject.GetComponent<Animator>().SetBool("damaged", true);
            collision.gameObject.GetComponent<playerController>().health -= damage;
        }
        Destroy(gameObject);
    }

}
