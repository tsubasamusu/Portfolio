using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float enemyHP;

    private float counter;

    private bool flag;

    private GameObject label;

    EnemyLabel enemyLabel;

    private void Start()
    {
        this.label = GameObject.Find("EnemyLabel");

        this.enemyLabel = this.label.GetComponent<EnemyLabel>();
    }

    private void Update()
    {
        if (this.counter >= this.enemyHP)
        {
            if (this.flag == false)
            {
                this.enemyLabel.deadEnemyNumber++;

                this.flag = true;
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Shell")
        {
            this.counter++;
        }
    }
}