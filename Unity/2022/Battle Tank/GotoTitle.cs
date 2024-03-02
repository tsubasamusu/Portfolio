using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoTitle : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Title");
    }
}