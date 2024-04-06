using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplanController : MonoBehaviour
{
    public GameObject tank;

    TankHealth tankHealth;

    [SerializeField]
    private float speed;

    public bool goFlag;

    void Start()
    {
        this.tankHealth = this.tank.GetComponent<TankHealth>();
    }

    void Update()
    {
        if (this.tankHealth.enableFlag == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.goFlag = true;
            }
        }

        if (this.goFlag == true)
        {
            transform.Translate(0, 0, this.speed);
        }
    }
}