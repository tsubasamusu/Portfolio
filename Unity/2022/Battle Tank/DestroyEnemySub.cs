using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemySub : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private GameObject effectPrefab2;

    public int objectHP;

    [SerializeField]
    private GameObject itemPrefab1;

    [SerializeField]
    private GameObject itemPrefab2;

    [SerializeField]
    private GameObject itemPrefab3;

    [SerializeField]
    private int scoreValue;

    private ScoreManager sm;

    private GameObject itemPrefab;

    void Start()
    {
        sm = GameObject.Find("ScoreLabel").GetComponent<ScoreManager>();
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

                int px = Random.Range(0, 3);

                switch (px)
                {
                    case 0: itemPrefab = itemPrefab1; break;

                    case 1: itemPrefab = itemPrefab2; break;

                    case 2: itemPrefab = itemPrefab3; break;
                }

                Vector3 pos = transform.position;

                sm.AddScore(scoreValue);

                Destroy(gameObject);
            }
        }
    }
}