using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab1;

    [SerializeField]
    private GameObject effectPrefab2;

    public int tankHP;

    public int tankMaxHP;

    [SerializeField]
    private Text HPLabel;

    [SerializeField]
    private Text aLabel;

    [SerializeField]
    private AudioClip alarm;

    private float time;

    private AudioSource aud;

    private bool soundFlag;

    private bool flag;

    public bool enableFlag;

    [SerializeField]
    private GameObject airplain;

    AirplanController airplanController;

    void Start()
    {
        tankHP = tankMaxHP;

        HPLabel.text = "HP:" + tankHP;

        aud = GetComponent<AudioSource>();

        this.airplanController = this.airplain.GetComponent<AirplanController>();
    }

    private void Update()
    {
        if (tankHP <= 2)
        {
            this.enableFlag = true;

            if (this.flag == false)
            {
                this.soundFlag = true;

                this.flag = true;
            }

            if (airplanController.goFlag == false)
            {
                this.aLabel.text = "Tap 'A' To Start\n" + "Aerial Bombing";
            }
            else
            {
                this.aLabel.text = "";
            }

            this.time += Time.deltaTime;

            if (this.time >= 4.0f)
            {
                this.soundFlag = true;

                this.time = 0;
            }
        }

        if (this.soundFlag == true)
        {
            this.aud.PlayOneShot(this.alarm);

            this.soundFlag = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyShell")
        {
            tankHP -= 1;

            HPLabel.text = "HP:" + tankHP;

            Destroy(other.gameObject);

            if (tankHP > 0)
            {
                GameObject effect1 = Instantiate(effectPrefab1, transform.position, Quaternion.identity);

                Destroy(effect1, 1.0f);
            }
            else
            {
                GameObject effect2 = Instantiate(effectPrefab2, transform.position, Quaternion.identity);

                Destroy(effect2, 1.0f);

                this.gameObject.SetActive(false);

                Invoke("GoToGameOver", 0.1f);
            }
        }
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void AddHP(int amount)
    {
        tankHP = Mathf.Clamp(tankHP + amount, 0, tankMaxHP);

        HPLabel.text = "HP:" + tankHP;
    }
}