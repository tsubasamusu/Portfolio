using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private GameManager gameManager;

    public void SetUpGoalController(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BodyController bodyController))
        {
            gameManager.PrepareGameClear();

            bodyController.enabled = false;
        }
    }
}