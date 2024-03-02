using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField]
    private Rigidbody grenadePrefab;

    [SerializeField]
    private AudioClip throwSound;

    [SerializeField]
    private float throwSpeed;

    [SerializeField]
    private KeyCode throwGrenadeKey;

    [SerializeField, Header("ŠŽ”")]
    private int grenadeCount;

    [SerializeField]
    private Text txtGrenadeCount;

    private void Start()
    {
        UpdateGrenadeCount();
    }

    void Update()
    {
        ControlThrowingGrenade();
    }

    private void ControlThrowingGrenade()
    {
        if (Input.GetKeyDown(throwGrenadeKey) && grenadeCount > 0)
        {
            grenadeCount -= 1;

            UpdateGrenadeCount();

            Rigidbody grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);

            grenade.AddForce(transform.forward * throwSpeed);

            AudioSource.PlayClipAtPoint(throwSound, Camera.main.transform.position);
        }
    }

    private void UpdateGrenadeCount()
    {
        txtGrenadeCount.text = grenadeCount.ToString();
    }
}