using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Vector3 angle;

    private AudioSource audioS;

    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        angle = transform.eulerAngles;

        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            UpDownTurret();

            angle.x -= this.moveSpeed * 0.01f;

            if (angle.x <= -60)
            {
                angle.x = -60;
            }
        }
        else if (Input.GetKey(KeyCode.V))
        {
            UpDownTurret();

            angle.x += this.moveSpeed * 0.01f;

            if (angle.x >= 0)
            {
                angle.x = 0;
            }
        }
        else
        {
            audioS.enabled = false;
        }
    }

    void UpDownTurret()
    {
        audioS.enabled = true;

        transform.eulerAngles = new Vector3(angle.x, transform.root.eulerAngles.y, 0);
    }
}