using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffectPrefab;

    [SerializeField]
    private AudioClip explosionSE;

    private AudioSource aud;

    void Start()
    {
        this.aud = GetComponent<AudioSource>();
    }

    void OnCollisionEnter()
    {
        GetComponent<MeshRenderer>().enabled = false;

        this.aud.PlayOneShot(this.explosionSE);

        GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        Destroy(effect, 3.0f);

        Destroy(gameObject, 3.0f);
    }
}