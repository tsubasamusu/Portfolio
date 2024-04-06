using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExposionController : MonoBehaviour
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
        if (other.CompareTag("Shell"))
        {
            this.audioS.PlayOneShot(this.explosionSE);
        }
    }
}