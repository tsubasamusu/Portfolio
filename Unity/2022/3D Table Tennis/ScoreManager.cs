using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int count;

    private OwnerType server;

    public void SetUpScoreManager(BallController ballController, UIManager uIManager, PlayerController playerController)
    {
        StartCoroutine(CheckScore());

        IEnumerator CheckScore()
        {
            while (true)
            {
                if (ballController.transform.position.y > 0.25f)
                {
                    yield return null;

                    continue;
                }

                UpdateScore(GetUpadateValue(ballController), uIManager);

                ballController.PrepareRestartGame(GetAppropriatServer(), playerController);

                yield return null;
            }
        }
    }

    private OwnerType GetAppropriatServer()
    {
        count++;

        if (count < 2)
        {
            return server;
        }

        count = 0;

        return server = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
    }

    private void UpdateScore((int playerUpdateValue, int enemyUpdateValue) updateValue, UIManager uIManager)
    {
        SoundManager.instance.PlaySound(updateValue.playerUpdateValue > 0 ? SoundDataSO.SoundName.PlayerPointSE : SoundDataSO.SoundName.EnemyPointSE);

        GameData.instance.score.playerScore += updateValue.playerUpdateValue;

        GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;

        uIManager.PrepareUpdateTxtScore();
    }

    private (int playerUpdateValue, int enemyUpdateValue) GetUpadateValue(BallController ballController)
    {
        return ballController.InCourt ?
            ballController.CurrentOwner == OwnerType.Player ? (1, 0) : (0, 1)
            : ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);
    }
}