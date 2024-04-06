using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DestroyEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private GameObject effectPrefab2;

    public int objectHP;

    [SerializeField]
    private GameObject enemyHPLabel;

    private void Update()
    {
        this.enemyHPLabel.GetComponent<Text>().text = "EnemyHP:" + this.objectHP.ToString("F0");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shell"))
        {
            objectHP -= 1;

            if (objectHP > 0)
            {
                Destroy(other.gameObject);

                GameObject effect = Instantiate(effectPrefab, other.transform.position, Quaternion.identity);

                Destroy(effect, 2.0f);
            }
            else
            {
                objectHP = 0;

                Destroy(other.gameObject);

                GameObject effect2 = Instantiate(effectPrefab2, other.transform.position, Quaternion.identity);

                Destroy(effect2, 2.0f);

                Destroy(gameObject);

                SceneManager.LoadScene("GameClear");
            }
        }
    }
}