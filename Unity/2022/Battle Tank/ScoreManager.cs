using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    private Text scoreLabel;

    void Start()
    {
        scoreLabel = GetComponent<Text>();

        scoreLabel.text = score + "point";
    }

    public void AddScore(int amount)
    {
        score += amount;

        scoreLabel.text = score + "point";
    }
}