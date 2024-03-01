using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGet : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().text = ScoreManager.score.ToString("F0") + "point";
    }
}