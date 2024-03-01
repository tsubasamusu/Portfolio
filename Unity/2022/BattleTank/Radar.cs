using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    private Vector3 angle;

    public bool stopFlag;

    void Start()
    {
        angle = transform.eulerAngles;
    }

    void Update()
    {
        if (this.stopFlag == false)
        {
            TurnRader();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.stopFlag = true;
        }
        else
        {
            TurnRader();

            this.stopFlag = false;
        }
    }

    void TurnRader()
    {
        angle.y += 2.0f;

        transform.eulerAngles = new Vector3(transform.root.eulerAngles.x - 90, angle.y, transform.root.eulerAngles.z);
    }
}