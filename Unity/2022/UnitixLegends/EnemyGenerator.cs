using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField]
        private EnemyController enemyPrefab;

        [SerializeField]
        private int maxGenerateCount;

        [SerializeField]
        private float flightTime;

        [SerializeField]
        private Transform enemiesTran;

        [HideInInspector]
        public List<EnemyController> generatedEnemyList = new List<EnemyController>();

        private List<float> generateTimeList = new List<float>();

        private float timer;

        private int generateCount;

        public IEnumerator GenerateEnemy(UIManager uIManager, PlayerController player)
        {
            for (int i = 0; i < maxGenerateCount; i++)
            {
                generateTimeList.Add(Random.Range(1f, flightTime));
            }

            while (true)
            {
                timer += Time.deltaTime;

                for (int i = 0; i < generateTimeList.Count; i++)
                {
                    if (timer >= generateTimeList[i])
                    {
                        EnemyController enemy = Instantiate(enemyPrefab, transform.position, enemiesTran.rotation);

                        enemy.SetUpEnemy(uIManager, this, player, generateCount);

                        generatedEnemyList.Add(enemy);

                        generateCount++;

                        uIManager.UpdateTxtOtherCount(enemiesTran.childCount);

                        generateTimeList.RemoveAt(i);
                    }
                }

                yield return null;
            }
        }
    }
}