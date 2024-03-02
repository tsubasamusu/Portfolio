using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed;

    public float turnSpeed;

    private float velocity;

    private float velocity2;

    private float movementInputValue;

    private float turnInputValue;

    private float time;

    private Rigidbody rb;

    private Vector3 startPos;

    private Vector3 endPos;

    private Vector3 firstPos;

    private Vector3 rot;

    private Text r;

    private Text v;

    [SerializeField]
    private GameObject recoverLabel;

    [SerializeField]
    private GameObject velocityLabel;

    [SerializeField]
    private float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        this.firstPos = transform.position;

        this.r = this.recoverLabel.GetComponent<Text>();

        this.v = this.velocityLabel.GetComponent<Text>();
    }

    void Update()
    {
        TankMove();

        TankTurn();

        CalculateVelocity();

        Recover();
    }

    void TankMove()
    {
        movementInputValue = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * movementInputValue * moveSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
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
        float angleX = Mathf.Abs(transform.eulerAngles.x);

        float angleZ = Mathf.Abs(transform.eulerAngles.z);

        if (angleX >= this.angle || angleZ >= this.angle)
        {
            this.r.text = "Tap 'R' To\n" + "Recover";
        }
        else
        {
            this.r.text = "";
        }

        this.rot = transform.eulerAngles;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.eulerAngles = new Vector3(0, rot.y, 0);
        }

        if (transform.position.y <= 0)
        {
            transform.position = this.firstPos;
        }
    }

    void CalculateVelocity()
    {
        this.time += Time.deltaTime;

        if (this.time >= 0.1f)
        {
            this.startPos = transform.position;
        }
        if (this.time >= 0.2f)
        {
            this.endPos = transform.position;

            this.time = 0;
        }

        Vector3 dir = endPos - startPos;

        float length = dir.magnitude;

        this.velocity = length * 10f * 60 * 60;

        if (this.velocity > 0)
        {
            this.velocity2 = this.velocity / 1000f;
        }

        this.v.text = this.velocity2.ToString("F0") + "km/h";
    }
}