using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float targetSpan = 5.0f;

    float delta = 0;

    int px;

    public AudioClip clip;

    public TimeController timeController;

    AudioSource aud;

    void Start()
    {
        this.aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timeController.timeLimit > 0)
        {
            this.delta += Time.deltaTime;

            if (this.delta > this.targetSpan)
            {
                this.delta = 0;

                this.px = Random.Range(-20, 21);
            }

            transform.position = new Vector3(this.px, 0, 10);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        this.aud.PlayOneShot(this.clip);
    }
}