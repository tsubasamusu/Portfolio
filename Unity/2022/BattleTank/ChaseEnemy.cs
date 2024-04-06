using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    private GameObject target;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        this.target = GameObject.Find("Tank");

    }

    void Update()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;
        }
    }
}