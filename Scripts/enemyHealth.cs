using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float health;

    public GameObject bob;

    private void Update()
    {
        if(
            health <= 0 &&
            gameObject.GetComponent<bossBMovement>() == null
            ) //die if not on bossB object
        {
            if(gameObject.transform.position.x == 97.5f && gameObject.transform.position.y == 0.5f) {
                Destroy(GameObject.Find("Grid2"));
            }

            die();
        }
    }

    void die()
    {
        Debug.Log("ded");
        Instantiate(bob, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
