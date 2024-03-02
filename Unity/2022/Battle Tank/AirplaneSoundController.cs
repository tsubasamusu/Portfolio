using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneSoundController : MonoBehaviour
{
    public GameObject tank;

    [SerializeField]
    private GameObject airplain;

    AirplanController airplanController;

    private AudioSource aud;

    public AudioClip se;

    private bool soundFlag;

    void Start()
    {
        this.airplanController = this.airplain.GetComponent<AirplanController>();

        this.aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (this.airplanController.goFlag == true && this.soundFlag == false)
        {
            this.aud.PlayOneShot(this.se);

            this.soundFlag = true;
        }
    }
}