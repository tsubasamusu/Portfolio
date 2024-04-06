using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotShell : MonoBehaviour
{
    public float shotSpeed;

    private float time;

    private bool shotFlag;

    [SerializeField]
    private GameObject shellPrefab;

    private GameObject tank;

    [SerializeField]
    private AudioClip shotSound;

    [SerializeField]
    private float shotSpan;

    [SerializeField]
    private float range;

    private void Start()
    {
        this.tank = GameObject.Find("Tank");
    }

    void Update()
    {
        CalculateLength();

        this.time += Time.deltaTime;

        if (this.shotFlag == true && this.time >= this.shotSpan)
        {
            this.time = 0;

            GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);

            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            shellRb.AddForce(transform.forward * shotSpeed);

            Destroy(shell, 3.0f);

            AudioSource.PlayClipAtPoint(shotSound, transform.position);
        }
    }

    void CalculateLength()
    {
        Vector3 dir = this.tank.transform.position - transform.position;

        float length = dir.magnitude;

        if (length <= this.range)
        {
            this.shotFlag = true;
        }
        else
        {
            this.shotFlag = false;
        }
    }
}