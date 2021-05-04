using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet2 : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject gun;
    public float bulletDamage;

    private float aliveTime;
    private float maxAliveTime = 7f;
    private Vector2 bulletForce = new Vector2(50f, 0f);

    private float abs(float a) {
        if(a < 0) return -a;
        return a;
    }
    private void Start()
    {
        Vector3 mousePos = Input.mousePosition;
        //GameObject p = GameObject.Find("Player");
        Vector3 objectPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        mousePos.z = 5.23f;
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x);

        if (abs((angle * Mathf.Rad2Deg) - 90f) < 0.5f)
        {
            bulletForce.x = 0f;
            bulletForce.y = 50f;
        }

        float mult = 1f;
        if (angle * Mathf.Rad2Deg > 90 || angle * Mathf.Rad2Deg < -90) mult = -1f;

        bulletForce.x = mult * 50f * Mathf.Cos(angle);
        bulletForce.y = mult * 50f * Mathf.Sin(angle);

        aliveTime = 0;
        rb = GetComponent<Rigidbody2D>();
        gun = GameObject.Find("gun2(Clone)");

        rb.AddForce(bulletForce * (gun.transform.localScale.x > 0 ? 1 : -1), ForceMode2D.Impulse);
        SoundManager.PlaySound("bullet_shoot");
    }

    private void Update()
    {
        aliveTime += Time.fixedDeltaTime;
        if (aliveTime > maxAliveTime) die();
    }

    void die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<enemyHealth>() != null)
        {
            if (collision.gameObject.GetComponent<Animator>() != null)
                collision.gameObject.GetComponent<Animator>().SetBool("damaged", true);
            collision.gameObject.GetComponent<enemyHealth>().health -= bulletDamage;
        }
        die();
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<enemyHealth>() != null)
        {
            collision.gameObject.GetComponent<enemyHealth>().health -= bulletDamage;
            if (collision.gameObject.GetComponent<Animator>() != null)
                collision.gameObject.GetComponent<Animator>().SetBool("damaged", true);
        }
        die();  
    }
}
