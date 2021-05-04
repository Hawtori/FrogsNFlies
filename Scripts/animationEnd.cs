using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(
            gameObject.GetComponent<SpriteRenderer>().sprite.name == "Explosion_3" || 
            gameObject.GetComponent<SpriteRenderer>().sprite.name == "JumpCharge_3")
            Destroy(gameObject);
    }
}
