using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposionController : MonoBehaviour
{
    private AudioSource audioS;

    [SerializeField]
    private AudioClip explosionSE;

    void Start()
    {
        this.audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shell") || other.CompareTag("EnemyShell"))
        {
            this.audioS.PlayOneShot(this.explosionSE);
        }
    }
}