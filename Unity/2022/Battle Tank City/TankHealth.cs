using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TankHealth : MonoBehaviour
{
    public static float damagePercentage;

    [SerializeField]
    private GameObject hpGauge;

    private void Update()
    {
        if (this.hpGauge.GetComponent<Image>().fillAmount <= 0.001)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "EnemyShell")
        {
            this.hpGauge.GetComponent<Image>().fillAmount -= damagePercentage * 0.01f;
        }
    }
}