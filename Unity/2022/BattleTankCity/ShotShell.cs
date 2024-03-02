using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotShell : MonoBehaviour
{
    public float shotSpeed;

    private float time;

    private Rigidbody rb;

    private Text shellConditionText;

    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private GameObject shellConditionLabel;

    [SerializeField]
    private AudioClip shotSound;

    [SerializeField]
    private float shotSpan;

    [SerializeField]
    private GameObject tank;

    [SerializeField]
    private Vector3 reaction;

    private void Start()
    {
        this.rb = this.tank.GetComponent<Rigidbody>();

        this.shellConditionText = this.shellConditionLabel.GetComponent<Text>();
    }

    void Update()
    {
        this.time += Time.deltaTime;

        if (this.time > this.shotSpan)
        {
            this.shellConditionText.text = "Completion";
        }
        else
        {
            this.shellConditionText.text = "Loading";
        }

        if (Input.GetKeyDown(KeyCode.Space) && this.time >= this.shotSpan)
        {
            this.time = 0;

            GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);

            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            shellRb.AddForce(transform.forward * shotSpeed);

            this.rb.AddForce(this.reaction * 100000000f, ForceMode.Impulse);

            Destroy(shell, 3.0f);

            AudioSource.PlayClipAtPoint(shotSound, transform.position);
        }
    }
}