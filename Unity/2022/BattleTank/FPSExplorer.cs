using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSExplorer : MonoBehaviour
{
    private float delta;

    private float fpsCownter;

    void Update()
    {
        this.delta += Time.deltaTime;

        this.fpsCownter++;

        if (this.delta >= 1.0f)
        {
            this.delta = 0;

            GetComponent<Text>().text = this.fpsCownter + "FPS";

            this.fpsCownter = 0;
        }
    }
}