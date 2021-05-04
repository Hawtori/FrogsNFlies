using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorMovement : MonoBehaviour
{
    public GameObject stop;
    public Rigidbody2D rb;

    private Vector3 initPos;
    private float speed = 10f;
    private bool movingupEh = false;

    private int callToEnter = 0;
    private int callToExit = 0;

    private void Start()
    {
        initPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (movingupEh) moveElevator();
        else moveElevatorBack();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        StopAllCoroutines();
    //        StartCoroutine(moveElevatorCall(1));
    //        Debug.Log("Entered elevator: " + callToEnter++);
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        StopAllCoroutines();
    //        StartCoroutine(moveElevatorCall(2));
    //        Debug.Log("Exited elevator: " + callToExit++);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(moveElevatorCall(1));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(moveElevatorCall(2));
        }
    }



    void moveElevator()
    {
        //move towards stop position
        Vector3 movement = stop.transform.position - transform.position;
        movement.Normalize();
        rb.velocity = movement * speed;
        if(transform.position.y > stop.transform.position.y)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void moveElevatorBack()
    {
        //move towards init position
        Vector3 movement = initPos - transform.position;
        movement.Normalize();
        rb.velocity = movement * speed;
        if (transform.position.y < initPos.y)
        {
            rb.velocity = Vector3.zero;
        }
    }

    IEnumerator moveElevatorCall(int num)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (num == 1) movingupEh = true;
        yield return new WaitForSecondsRealtime(1.5f);
        if (num == 2) movingupEh = false;
    }
}
