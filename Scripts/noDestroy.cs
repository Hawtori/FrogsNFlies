using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noDestroy : MonoBehaviour
{
    private static bool e;
    void Start ()
    {
        if (!e)
        {
            e = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
