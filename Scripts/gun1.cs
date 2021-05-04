using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun1 : MonoBehaviour
{
    public GameObject bullet;
    public playerController _playerController;

    private float shootTimer;

    private void Start()
    {
        shootTimer = 2f;
        _playerController = gameObject.GetComponentInParent<playerController>();
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        GameObject p = GameObject.Find("Player");
 
        Vector3 objectPos = Camera.main.WorldToScreenPoint(p.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
  
        if(angle > 90 || angle < -90) {
            angle += 180;
            if (gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(shootTimer > 0)
        {
            shootTimer -= Time.fixedDeltaTime;
        }
        if (Input.GetMouseButtonDown(0) && shootTimer <= 0f) //righ click
        {
            spawnBullet();
            shootTimer = 2f;
        }
    }

    void spawnBullet()
    {
        Quaternion rotation = transform.rotation * Quaternion.AngleAxis(-90 * _playerController.facingDir, Vector3.forward);
        Vector3 mousePos = Input.mousePosition;
        mousePos.Normalize();
        Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(bullet, offset, rotation);
    }
}
