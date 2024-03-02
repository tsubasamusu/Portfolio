using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
    float finalScore = 0;

    float bestScore = 0;

    public bool Flag;

    private Text scoreText;

    private void Start()
    {
        this.scoreText = GetComponent<Text>();
    }

    public void AddScore(float score)
    {
        Debug.Log(score);

        if (this.Flag == true)
        {
            this.finalScore += score;

            this.Flag = false;

            if (score > this.bestScore)
            {
                this.bestScore = score;
            }
        }

        this.scoreText.text = "Final Score\n" + this.finalScore.ToString("F0") + "point\n" + "Best Score\n" + this.bestScore.ToString("F0") + "point";
    }
}