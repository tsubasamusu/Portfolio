using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -1.0f)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}