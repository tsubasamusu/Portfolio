using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyPrefab;

    [SerializeField]
    private Transform[] enemyGenerateTrans;

    public List<EnemyController> enemyList = new();

    [SerializeField]
    private int maxEnemyCount;

    [SerializeField]
    private float GenerateSpan;

    private int generateCount;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= GenerateSpan && generateCount < maxEnemyCount)
        {
            EnemyController enemy = Instantiate(enemyPrefab, enemyGenerateTrans[Random.Range(0, enemyGenerateTrans.Length)].position, Quaternion.identity);

            enemyList.Add(enemy);

            generateCount++;

            timer = 0;
        }
    }
}