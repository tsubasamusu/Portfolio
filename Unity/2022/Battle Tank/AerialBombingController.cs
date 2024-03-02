using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialBombingController : MonoBehaviour
{
    public float shotSpeed;

    [SerializeField]
    private GameObject shellPrefab;

    private float timer;

    [SerializeField]
    private float timeBetweenShot;

    public GameObject tank;

    TankHealth tankHealth;

    private int count;

    private bool attackFlag;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject airplain;

    private bool attackAllowFlag = true;

    AirplanController airplanController;

    void Start()
    {
        this.tankHealth = this.tank.GetComponent<TankHealth>();

        this.airplanController = this.airplain.GetComponent<AirplanController>();
    }

    void Update()
    {
        if (airplanController.goFlag == true)
        {
            timer += Time.deltaTime;

            if (this.timer >= 2.0f && this.attackAllowFlag == true)
            {
                this.attackFlag = true;
            }

            if (this.attackFlag == true && timer > timeBetweenShot)
            {
                timer = 0.0f;

                GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);

                Rigidbody shellRb = shell.GetComponent<Rigidbody>();

                shellRb.AddForce(transform.forward * shotSpeed);

                this.count++;
            }

            if (this.count >= 20)
            {
                this.attackFlag = false;

                this.attackAllowFlag = false;
            }
        }
    }
}