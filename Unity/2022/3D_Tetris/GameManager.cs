using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;

    private bool isGameEnd;

    private IEnumerator Start()
    {
        yield return UIManager.instance.PlayGameStart();

        blockGenerator.SetUpBlockGenerator();

        UIManager.instance.PrepareCheck();

        BlockManager.instance.CurrentBlock = blockGenerator.GenerateBlock();

        BlockManager.instance.SetUpBlockManager(this);

        UIManager.instance.UpdateTxtScore(0);

        StartCoroutine(ReduceTimeLimit());

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BGM, true);
    }

    private IEnumerator ReduceTimeLimit()
    {
        bool changedColor = false;

        float timeLimit = GameData.instance.TimeLimit;

        while (true)
        {
            if (isGameEnd)
            {
                break;
            }

            if (timeLimit <= 0f)
            {
                StartCoroutine(GameClear());

                break;
            }

            if (timeLimit < 10f && !changedColor)
            {
                UIManager.instance.SetTxtTimeLimitColor(Color.red);

                SoundManager.instance.PlaySound(SoundDataSO.SoundName.TenTimeLimitSE);

                changedColor = true;
            }

            timeLimit -= Time.deltaTime;

            UIManager.instance.SetTxtTimeLimit(timeLimit);

            yield return null;
        }
    }

    public void PrepareGameOver()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        PrepareGameEnd();

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameOverSE);

        yield return UIManager.instance.PlayGameOver();

        SceneManager.LoadScene("Main");
    }

    private IEnumerator GameClear()
    {
        PrepareGameEnd();

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameClearSE);

        yield return UIManager.instance.PlayGameClear();

        SceneManager.LoadScene("Main");
    }

    private void PrepareGameEnd()
    {
        isGameEnd = true;

        blockGenerator.StopGenerateBlock();

        Destroy(BlockManager.instance.CurrentBlock);

        Cursor.visible = true;

        SoundManager.instance.StopSound(0.5f);
    }
}