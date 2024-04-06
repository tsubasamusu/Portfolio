using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public static float timeLimit;

    private Text label;

    void Start()
    {
        this.label = GetComponent<Text>();
    }

    void Update()
    {
        this.label.text = (timeLimit -= Time.deltaTime).ToString("F1");

        if (timeLimit <= 0.0f)
        {
            timeLimit = 0.0f;

            SceneManager.LoadScene("GameOver");
        }
    }
}