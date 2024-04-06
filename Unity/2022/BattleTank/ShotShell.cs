using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotShell : MonoBehaviour
{
    public float shotSpeed;

    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private AudioClip shotSound;

    private float timer;

    public int shotCount;

    public int shotMaxCount;

    [SerializeField]
    private float timeBetweenShot;

    [SerializeField]
    private Text shellLabel;

    [SerializeField]
    private Text ShellConditionLabel;

    void Start()
    {
        shotCount = shotMaxCount;

        shellLabel.text = "ShellÅF" + shotCount;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timer > timeBetweenShot && shotCount > 0)
        {
            timer = 0.0f;

            shotCount -= 1;

            shellLabel.text = "ShellÅF" + shotCount;

            GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);

            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            shellRb.AddForce(transform.forward * shotSpeed);

            Destroy(shell, 3.0f);

            AudioSource.PlayClipAtPoint(shotSound, transform.position);
        }

        if (this.timer > timeBetweenShot)
        {
            this.ShellConditionLabel.text = "Completion";
        }
        else
        {
            this.ShellConditionLabel.text = "Loading";
        }
    }

    public void AddShell(int amount)
    {
        shotCount = Mathf.Clamp(shotCount + amount, 0, shotMaxCount);

        shellLabel.text = "ShellÅF" + shotCount;
    }
}