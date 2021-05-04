using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<playerController>().health = 0f;
        }
        else
            Destroy(collision.gameObject);
    }
}
