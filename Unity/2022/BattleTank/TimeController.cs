using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeLimit;

    private float timeLimit2;

    private float time;

    void Start()
    {
        this.timeLimit2 = this.timeLimit;
    }

    void Update()
    {
        this.time += Time.deltaTime;

        this.timeLimit = this.timeLimit2 - this.time;

        GetComponent<Text>().text = this.timeLimit.ToString("F1");

        if (this.timeLimit <= 0)
        {
            SceneManager.LoadScene("SubClear");
        }
    }
}