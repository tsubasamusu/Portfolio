using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterRader : MonoBehaviour
{
    private Transform target;

    private GameObject tank;

    private void Start()
    {
        this.tank = GameObject.Find("Tank");
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.tank.TryGetComponent<Transform>(out this.target))
        {
            if (other.CompareTag("Player"))
            {
                transform.root.LookAt(target);
            }
        }
    }
}