using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Camera highCamera;

    [SerializeField]
    private GameObject aimImage;

    private bool mainCameraON = true;

    void Start()
    {
        mainCamera.enabled = true;

        highCamera.enabled = false;

        aimImage.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && mainCameraON == true)
        {
            mainCamera.enabled = false;

            highCamera.enabled = true;

            mainCameraON = false;

            aimImage.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.C) && mainCameraON == false)
        {
            mainCamera.enabled = true;

            highCamera.enabled = false;

            mainCameraON = true;

            aimImage.SetActive(true);
        }
    }
}