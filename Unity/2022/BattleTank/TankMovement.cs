using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed;

    public float turnSpeed;

    private Rigidbody rb;

    private float movementInputValue;

    private float turnInputValue;

    private Vector3 rot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        TankMove();

        TankTurn();

        Recover();

        if (transform.position.y < -1.0f)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        }
    }

    void TankMove()
    {
        movementInputValue = Input.GetAxis("Vertical");

        transform.Translate(this.moveSpeed * this.movementInputValue * Time.deltaTime, 0, 0);
    }

    void TankTurn()
    {
        turnInputValue = Input.GetAxis("Horizontal");

        float turn = turnInputValue * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);

        rb.MoveRotation(rb.rotation * turnRotation);

    }

    void Recover()
    {
        rot = transform.eulerAngles;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.eulerAngles = new Vector3(0, rot.y, 0);
        }
    }
}