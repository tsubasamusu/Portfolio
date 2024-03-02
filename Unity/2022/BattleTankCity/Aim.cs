using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    [SerializeField]
    private Image aimImage;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(transform.position, transform.forward * 60, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 60))
        {
            string hitName = hit.transform.gameObject.tag;

            if (hitName == "Enemy")
            {
                aimImage.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                aimImage.color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            aimImage.color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}