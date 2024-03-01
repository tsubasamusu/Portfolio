using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShotShell : MonoBehaviour
{
    public float shotSpeed;

    [SerializeField]
    private GameObject enemyShellPrefab;

    [SerializeField]
    private AudioClip shotSound;

    [SerializeField]
    private float EnemyShotShellSpan;

    [SerializeField]
    private Text stopLabel;

    private float delta;

    private float stopTimer;

    public GameObject turret;

    private Radar rader;

    void Start()
    {
        this.rader = this.turret.GetComponent<Radar>();
    }

    void Update()
    {
        this.delta += Time.deltaTime;

        stopTimer -= Time.deltaTime;

        if (stopTimer < 0)
        {
            stopTimer = 0;
        }

        if (this.stopTimer > 0)
        {
            stopLabel.text = stopTimer.ToString("0");
        }
        else
        {
            stopLabel.text = "";
        }

        if (this.delta >= this.EnemyShotShellSpan && stopTimer <= 0 && this.rader.stopFlag == true)
        {
            this.delta = 0;

            GameObject enemyShell = Instantiate(enemyShellPrefab, transform.position, Quaternion.identity);

            Rigidbody enemyShellRb = enemyShell.GetComponent<Rigidbody>();

            enemyShellRb.AddForce(transform.forward * shotSpeed);

            AudioSource.PlayClipAtPoint(shotSound, transform.position);

            Destroy(enemyShell, 3.0f);
        }
    }

    public void AddStopTimer(float amount)
    {
        stopTimer += amount;
    }
}