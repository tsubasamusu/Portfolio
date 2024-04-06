using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<ControllerBase> controllersList = new();

    [SerializeField]
    private BallController ballController;

    [SerializeField]
    private ScoreManager scoreManager;

    [SerializeField]
    private UIManager uiManager;

    private PlayerController playerController;

    private EnemyController enemyController;

    private IEnumerator Start()
    {
        SetUpControllers();

        scoreManager.SetUpScoreManager(ballController, uiManager, playerController);

        yield return uiManager.PlayGameStart();

        playerController.enabled = enemyController.enabled = true;

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);
    }

    private void SetUpControllers()
    {
        for (int i = 0; i < controllersList.Count; i++)
        {
            controllersList[i].SetUpControllerBase();

            if (controllersList[i].TryGetComponent(out PlayerController playerController))
            {
                this.playerController = playerController;

                playerController.SetUpPlayerController();

                ballController.SetUpBallController();
            }
            else if (controllersList[i].TryGetComponent(out EnemyController enemyController))
            {
                this.enemyController = enemyController;

                enemyController.SetUpEnemyController(ballController);
            }
        }

        playerController.enabled = enemyController.enabled = false;
    }

    private IEnumerator PlayGameEndPerformance(bool isGameOverPerformance)
    {
        yield return new WaitForSeconds(GameData.instance.FadeOutTime);

        SoundManager.instance.PlaySound(isGameOverPerformance ? SoundDataSO.SoundName.GameOverSE : SoundDataSO.SoundName.PlayerPointSE);

        yield return isGameOverPerformance ? uiManager.PlayGameOver() : uiManager.PlayGameClear();

        SceneManager.LoadScene("Main");
    }

    private void PrepareGameEnd()
    {
        playerController.enabled = enemyController.enabled = false;

        SoundManager.instance.StopSound();
    }

    private bool flag;

    private void Update()
    {
        int matchPoints = GameData.instance.MaxScore - 1;

        if (GameData.instance.score.playerScore <= matchPoints && GameData.instance.score.enemyScore <= matchPoints)
        {
            return;
        }

        if (!flag)
        {
            PrepareGameEnd();

            StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));

            flag = true;
        }
    }
}