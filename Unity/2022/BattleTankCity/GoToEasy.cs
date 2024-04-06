using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEasy : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        TankHealth.damagePercentage = 1.0f;

        TimeController.timeLimit = 1200.0f;

        SceneManager.LoadScene("Main");
    }
}