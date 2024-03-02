using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    GameObject timerText;

    GameObject pointText;

    GameObject generator;

    GameObject message;

    public BasketController basketController;

    public float time = 30.0f;

    public float clockSpan = 5.0f;

    public int point = 0;

    public void GetApple()
    {
        this.point += 100;
    }

    public void GetBomb()
    {
        this.point /= 2;
    }

    void Start()
    {
        this.generator = GameObject.Find("ItemGenerator");

        this.timerText = GameObject.Find("Time");

        this.pointText = GameObject.Find("Point");

        this.message = GameObject.Find("Message");
    }

    void Update()
    {

        this.time -= Time.deltaTime;

        if (basketController.clockFlag == true)
        {
            this.time += this.clockSpan;

            basketController.clockFlag = false;
        }

        if (this.time < 0)
        {
            this.timerText.GetComponent<Text>().text = "0.0";

            this.message.GetComponent<Text>().text = "Tap to Restart";

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("AppleCatchGameScene");
            }
        }
        else
        {
            this.message.GetComponent<Text>().text = "";

            this.timerText.GetComponent<Text>().text = this.time.ToString("F1");
        }

        this.pointText.GetComponent<Text>().text = this.point.ToString() + "point";
    }
}