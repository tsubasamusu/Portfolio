using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    float time = 0;

    public float timeLimit = 30f;

    float timeLimit2 = 0;

    float targetSpan2;

    GameObject t;

    GameObject message;

    public TargetController target;

    void Start()
    {
        this.t = GameObject.Find("Time");

        this.message = GameObject.Find("Message");

        this.timeLimit2 = this.timeLimit;

        this.targetSpan2 = target.targetSpan;
    }

    void Update()
    {
        float rate = this.timeLimit / this.timeLimit2;

        target.targetSpan = this.targetSpan2 * rate;

        if (this.time >= 0 && this.time < this.timeLimit2)
        {
            this.t.GetComponent<Text>().text = this.timeLimit.ToString("F1");

            this.message.GetComponent<Text>().text = "";
        }
        else if (this.time >= this.timeLimit2)
        {
            this.t.GetComponent<Text>().text = "0.0";

            this.message.GetComponent<Text>().text = "Tap To Restart";

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("IgaguriGameScene");
            }
        }

        this.time += Time.deltaTime;

        this.timeLimit = this.timeLimit2 - this.time;
    }
}