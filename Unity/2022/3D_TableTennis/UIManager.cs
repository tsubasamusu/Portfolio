using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private enum LogoType
    {
        Title,
        GameOver,
        GameClear
    }

    [Serializable]
    private class LogoData
    {
        public LogoType LogoType;
        public Sprite sprite;
    }

    [SerializeField]
    private List<LogoData> logoDatasList = new();

    [SerializeField]
    private Image imgLogo;

    [SerializeField]
    private Image imgBackground;

    [SerializeField]
    private CanvasGroup cgScore;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private CanvasGroup cgButton;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Image imgButton;

    [SerializeField]
    private Text txtButton;

    private Sprite GetLogoSprite(LogoType logoType)
    {
        return logoDatasList.Find(x => x.LogoType == logoType).sprite;
    }

    public IEnumerator PlayGameStart()
    {
        bool end = false;

        cgScore.alpha = 0f;

        imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);

        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);

        txtButton.text = "Start";

        button.onClick.AddListener(() => ClickedButton());

        button.interactable = false;

        imgLogo.DOFade(0f, 0f)
            .OnComplete(() => imgLogo.DOFade(1f, 1f)
            .OnComplete(() =>
            {
                imgButton.DOFade(1f, 1f);

                cgButton.DOFade(1f, 1f)
                    .OnComplete(() => button.interactable = true);
            }));

        void ClickedButton()
        {
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameStartSE);

            imgBackground.DOFade(0f, 1f);

            imgLogo.DOFade(0f, 1f);

            cgButton.DOFade(0f, 1f)
                .OnComplete(() => end = true);

            button.interactable = false;
        }

        yield return new WaitUntil(() => end == true);
    }

    public IEnumerator PlayGameOver()
    {
        bool end = false;

        imgBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);

        txtButton.text = "Restart";

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => ClickedButton());

        button.interactable = false;

        cgScore.DOFade(0f, 1f);

        imgLogo.DOFade(0f, 0f)

        .OnComplete(() => imgBackground.DOFade(1f, 1f)
            .OnComplete(() => imgLogo.DOFade(1f, 1f)
            .OnComplete(() =>
            {
                imgButton.DOFade(1f, 1f);

                cgButton.DOFade(1f, 1f)
                    .OnComplete(() => button.interactable = true);
            })));

        void ClickedButton()
        {
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);

            imgBackground.DOColor(Color.white, 1f);

            imgLogo.DOFade(0f, 1f);

            cgButton.DOFade(0f, 1f)
                .OnComplete(() => end = true);

            button.interactable = false;
        }

        yield return new WaitUntil(() => end == true);
    }

    public IEnumerator PlayGameClear()
    {
        bool end = false;

        imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);

        imgLogo.sprite = GetLogoSprite(LogoType.GameClear);

        imgButton.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);

        txtButton.text = "Restart";

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => ClickedButton());

        button.interactable = false;

        txtScore.DOColor(Color.blue, 2f);

        imgLogo.DOFade(0f, 0f)
            .OnComplete(() => imgBackground.DOFade(1f, 1f)
                .OnComplete(() => imgLogo.DOFade(1f, 1f)
                .OnComplete(() =>
                {
                    imgButton.DOFade(1f, 1f);

                    cgButton.DOFade(1f, 1f)
                        .OnComplete(() => button.interactable = true);
                })));

        void ClickedButton()
        {
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);

            cgScore.DOFade(0f, 1f);

            imgLogo.DOFade(0f, 1f);

            cgButton.DOFade(0f, 1f)
                .OnComplete(() => end = true);

            button.interactable = false;
        }

        yield return new WaitUntil(() => end == true);
    }

    public void PrepareUpdateTxtScore()
    {
        StartCoroutine(UpDateTxtScore());
    }

    private IEnumerator UpDateTxtScore()
    {
        txtScore.text = GameData.instance.score.playerScore.ToString() + ":" + GameData.instance.score.enemyScore.ToString();

        cgScore.DOFade(1f, 0.25f);

        yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreTime);

        if (GameData.instance.score.playerScore == GameData.instance.MaxScore)
        {
            yield break;
        }

        cgScore.DOFade(0f, 0.25f);
    }
}