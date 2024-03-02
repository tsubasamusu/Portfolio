using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankHealthSub : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab1;

    [SerializeField]
    private GameObject effectPrefab2;

    public int tankHP;

    public int tankMaxHP;

    [SerializeField]
    private Text HPLabel;

    void Start()
    {
        tankHP = tankMaxHP;

        HPLabel.text = "HP:" + tankHP;
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