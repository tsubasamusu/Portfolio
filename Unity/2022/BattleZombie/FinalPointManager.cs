using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalPointManager : MonoBehaviour
{
    [SerializeField]
    private Text txtFinalPoint;

    private void Start()
    {
        txtFinalPoint.text = PointManager.point.ToString();
    }
}