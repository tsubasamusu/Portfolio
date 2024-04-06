using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToHard : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        TankHealth.damagePercentage = 10.0f;

        TimeController.timeLimit = 300.0f;

        SceneManager.LoadScene("Main");
    }
}