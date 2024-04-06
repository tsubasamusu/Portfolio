using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tsubasa;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;

    [SerializeField]
    private BodyController bodyController;

    [SerializeField]
    private GoalController goalController;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private KeyCode restartKey;

    private bool isGameStart;

    private bool isGameClear;

    public bool IsGameStart
    {
        get
        {
            return isGameStart;
        }
    }

    public bool IsGameClear
    {
        get
        {
            return isGameClear;
        }
    }

    private IEnumerator Start()
    {
        SetUpGame();

        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.SoundName.GameStartSE));

        yield return StartCoroutine(uIManager.PlayGameStart());

        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.SoundName.MainBGM), true);

        StartCoroutine(uIManager.StartUpdateText());

        StartCoroutine(bodyController.StartControlBody());

        isGameStart = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            bodyController.ResetCharacterCondition(uIManager);

            SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.SoundName.RestartSE));
        }
    }

    private void SetUpGame()
    {
        cameraController.SetUpCameraController();

        uIManager.SetUpUI();

        goalController.SetUpGoalController(this);

        bodyController.SetUpBodyController();
    }

    public void PrepareGameClear()
    {
        StartCoroutine(SetGameClear());
    }

    private IEnumerator SetGameClear()
    {
        isGameClear = true;

        uIManager.StopUpdateText = true;

        SoundManager.instance.StopMainSound(0.5f);

        SoundManager.instance.PlaySound(SoundManager.instance.GetAudioClip(SoundManager.SoundName.GameClearSE));

        yield return StartCoroutine(uIManager.PlayGameClear());

        SceneManager.LoadScene("Main");
    }
}