using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed;

    void Update()
    {
        transform.Rotate(0, this.rotSpeed, 0);
    }
}