using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNormal : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        TankHealth.damagePercentage = 5.0f;

        TimeController.timeLimit = 600.0f;

        SceneManager.LoadScene("Main");
    }
}