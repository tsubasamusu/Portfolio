using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret2Controller : MonoBehaviour
{
    private GameObject enemyBody;

    void Start()
    {
        this.enemyBody = GameObject.Find("EnemyBody");
    }

    void Update()
    {
        transform.position = new Vector3(this.enemyBody.transform.position.x, this.enemyBody.transform.position.y + 0.3f, this.enemyBody.transform.position.z + 0.2f);
    }
}