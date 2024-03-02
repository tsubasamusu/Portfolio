using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    [SerializeField, Header("ŽË’ö‹——£")]
    private float tolerance;

    [SerializeField]
    private Image imgAim;

    void Update()
    {
        ChangeImgAimColor(CheckEnemy());
    }

    private bool CheckEnemy()
    {
        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit other, tolerance))
        {
            if (other.transform.gameObject.CompareTag("Enemy") || other.transform.gameObject.CompareTag("Grenade"))
            {
                return true;
            }
        }

        return false;
    }

    private void ChangeImgAimColor(bool isEnemy)
    {
        imgAim.color = isEnemy ? Color.red : Color.white;
    }
}