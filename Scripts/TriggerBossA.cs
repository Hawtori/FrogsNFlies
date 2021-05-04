using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossA : MonoBehaviour
{
    public GameObject bossA;
    public GameObject bossSpawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SoundManager.PlaySound("boss_spawn");
            GameObject boss = 
            Instantiate(bossA, bossSpawn.transform.position, Quaternion.identity);
            boss.transform.parent = GameObject.Find("===Enemies===").transform;
            // bossA.triggered = true;
            Destroy(gameObject);
        }
    }

}
