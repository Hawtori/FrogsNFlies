using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public int NumOfEnemies;
    public List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
    }

    private int randNum(int start, int end) {
        System.Random r = new System.Random();
        return r.Next(start, end);   //end is exclusive.
        //int range = 100;
        //double rDouble = r.NextDouble()* range; //for doubles
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.PlaySound("trigger");
            Destroy(gameObject);
            GameObject p = transform.parent.gameObject;
            int i = randNum(0, enemies.Count);
            //Debug.Log(collision.gameObject.tag);
            Instantiate(enemies[i], p.transform.position, Quaternion.identity);
            Destroy(p);
        }
    }
}


