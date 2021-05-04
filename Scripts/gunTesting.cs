using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunTesting : MonoBehaviour
{
    //gives the player a gun
    //to add more guns that players can get
    //create a prefab of the gun and attach a script to it, drag and drop that prefab onto this scripts list
    //create a gameobject with the sprite of the gun and a trigger collider
    //give the created gameobject the number of the gun (1, 2, 3,.. whatever type of gun it is) [ could have multiple guns with same num ]
    //add a case for that gun type (number of the gun) and basically copy pasta one of the switch cases, changing the name of the object name


    public List<GameObject> gunPrefabs;
    public GameObject player;
    public playerController pc;
    public int gunNum;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch (gunNum)
            {
                case 0:
                    if (GameObject.Find("gun(Clone)") == null) //if player doesnt have gun 1, give them gun 1
                    {
                        GameObject gun = Instantiate(gunPrefabs[gunNum], player.transform.position, Quaternion.identity);
                        gun.transform.parent = player.transform;
                        Vector3 pos = player.transform.position;
                        pos.x += 0.5f;
                        pos.y -= 0.15f;
                        pos.z -= 1;
                        gun.transform.position = pos;
                        if (pc.currentGun != -1) pc.guns[pc.currentGun].SetActive(false);
                        pc.guns.Add(gun);
                        pc.currentGun++;
                    }
                    break;
                case 1:
                    if(GameObject.Find("gun2(Clone)") == null) //if player doesnt have gun2 , give them gun 2
                    {
                        GameObject gun = Instantiate(gunPrefabs[gunNum], player.transform.position, Quaternion.identity);
                        gun.transform.parent = player.transform;
                        Vector3 pos = player.transform.position;
                        pos.x += 0.5f;
                        pos.y -= 0.15f;
                        pos.z -= 1;
                        gun.transform.position = pos;
                        if (pc.currentGun != -1) pc.guns[pc.currentGun].SetActive(false);
                        pc.guns.Add(gun);
                        pc.currentGun++;
                    }
                    break;
                  
                default:
                    break;
            }

            Destroy(gameObject);
        }
    }
}
