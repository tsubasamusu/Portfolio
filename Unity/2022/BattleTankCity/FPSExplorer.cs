using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSExplorer : MonoBehaviour
{
    private float time;

    private int counter;

    void Update()
    {
        this.counter++;

        this.time += Time.deltaTime;

        if (this.time >= 1.0f)
        {
            GetComponent<Text>().text = this.counter + "FPS";

            this.counter = 0;

            this.time = 0.0f;
        }
    }
}