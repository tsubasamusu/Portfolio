using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    private Text txtPoint;

    [HideInInspector]
    public int count;

    public static int point;

    public void UpdatePoint()
    {
        count++;

        point = count * 100;

        txtPoint.text = point.ToString();
    }
}