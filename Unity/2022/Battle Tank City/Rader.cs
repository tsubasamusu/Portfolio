using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rader : MonoBehaviour
{
    private Transform target;

    private GameObject tank;

    [SerializeField]
    private GameObject enemyBody;

    private void Start()
    {
        this.tank = GameObject.Find("Tank");
    }

    private void Update()
    {
        transform.position = new Vector3(this.enemyBody.transform.position.x, 1.6f, this.enemyBody.transform.position.z);

        transform.eulerAngles = new Vector3(0, this.enemyBody.transform.eulerAngles.y, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.tank.TryGetComponent<Transform>(out this.target))
        {
            if (other.CompareTag("Player"))
            {

            }
        }
    }
}