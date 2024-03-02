using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyLabel : MonoBehaviour
{
    [SerializeField]
    private int firstEnemyNumber;

    public int deadEnemyNumber;

    private Text labelText;

    void Start()
    {
        this.labelText = GetComponent<Text>();
    }


    void Update()
    {
        this.labelText.text = "Enemy\n" + (this.firstEnemyNumber - this.deadEnemyNumber).ToString();

        if ((this.firstEnemyNumber - this.deadEnemyNumber) <= 0)
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}