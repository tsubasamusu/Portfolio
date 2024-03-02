using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    public enum LogoType
    {
        Title,
        GameClear,
        GameOver
    }

    [Serializable]
    public class LogoData
    {
        public LogoType logoType;
        public Sprite sprLogo;
    }

    public static UIManager instance;

    [SerializeField]
    private Image imgBackGround;

    [SerializeField]
    private Image imgLogo;

    [SerializeField]
    private Image imgHold;

    [SerializeField]
    private Image imgButton;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtTimeLimit;

    [SerializeField]
    private Text txtButton;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Transform resultTran;

    [SerializeField]
    private Transform cameraTran;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image[] imgNextBlocks;

    [SerializeField]
    private List<LogoData> logoDatasList = new();

    private int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator PlayGameStart()
    {
        bool end = false;

        bool clicked = false;

        canvasGroup.alpha = 0f;

        button.interactable = false;

        imgBackGround.color = Color.white;

        imgButton.color = Color.blue;

        txtButton.text = string.Empty;

        imgBackGround.DOFade(1f, 0f);

        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        imgButton.DOFade(1f, 1f);

        imgLogo.DOFade(1f, 1f);

        txtButton.DOText("Game Start", 1f).OnComplete(() =>

        button.interactable = true);

        button.onClick.AddListener(() => ClickedButton());

        yield return new WaitUntil(() => clicked == true);

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnGameStartSE);

        Cursor.visible = false;

        imgBackGround.DOFade(0f, 1f);

        imgLogo.DOFade(0f, 1f);

        imgButton.DOFade(0f, 1f);

        txtButton.DOFade(0f, 1f).OnComplete(() =>

        end = true);

        yield return new WaitUntil(() => end == true);

        canvasGroup.alpha = 1f;

        imgHold.DOFade(1f, 0f);

        ClearImgHoldBlock();

        void ClickedButton()
        {
            clicked = true;

            button.interactable = false;
        }
    }

    public IEnumerator PlayGameOver()
    {
        bool end = false;

        bool clicked = false;

        canvasGroup.alpha = 0f;

        txtScore.DOFade(0f, 0f);

        imgBackGround.color = Color.black;

        txtButton.text = string.Empty;

        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        imgButton.color = Color.red;

        button.onClick.AddListener(() => ClickedButton());

        imgBackGround.DOFade(1f, 1f);

        imgLogo.DOFade(1f, 1f);

        txtButton.DOFade(1f, 0f).OnComplete(() =>

        txtButton.DOText("Restart", 1f));

        imgButton.DOFade(1f, 1f).OnComplete(() =>

        button.interactable = true);

        yield return new WaitUntil(() => clicked == true);

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnRestartSE);

        imgBackGround.DOColor(Color.white, 1f);

        imgLogo.DOFade(0f, 1f);

        imgButton.DOFade(0f, 1f);

        txtButton.DOFade(0f, 1f).OnComplete(() => end = true);

        yield return new WaitUntil(() => end == true);

        void ClickedButton()
        {
            clicked = true;

            button.interactable = false;
        }
    }

    public IEnumerator PlayGameClear()
    {
        bool end = false;

        bool clicked = false;

        canvasGroup.alpha = 0f;

        imgBackGround.color = Color.white;

        txtButton.text = string.Empty;

        imgLogo.sprite = GetLogoSprite(LogoType.GameClear);

        imgButton.color = Color.yellow;

        button.onClick.AddListener(() => ClickedButton());

        imgBackGround.DOFade(1f, 1f);

        imgLogo.DOFade(1f, 1f);

        imgButton.DOFade(1f, 1f);

        txtButton.DOFade(1f, 0f).OnComplete(() =>

        txtButton.DOText("Restart", 1f));

        txtScore.transform.DOMove(resultTran.position, 1f).OnComplete(() =>

        button.interactable = true);

        yield return new WaitUntil(() => clicked == true);

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnRestartSE);

        imgLogo.DOFade(0f, 1f);

        imgButton.DOFade(0f, 1f);

        txtScore.DOFade(0f, 1f);

        txtButton.DOFade(0f, 1f).OnComplete(() => end = true);

        yield return new WaitUntil(() => end == true);

        void ClickedButton()
        {
            clicked = true;

            button.interactable = false;
        }
    }

    public void UpdateTxtScore(int updateValue)
    {
        bool end = false;

        StartCoroutine(UpdateTxtScore());

        DOTween.To(() => score, (x) => score = x, score + updateValue, 0.5f).OnComplete(() => end = true);

        IEnumerator UpdateTxtScore()
        {
            while (!end)
            {
                txtScore.text = score.ToString() + "\npoint";

                yield return null;
            }
        }
    }

    public void SetTxtTimeLimit(float remainingTime)
    {
        txtTimeLimit.text = remainingTime.ToString("F1");
    }

    public void SetImgHoldBllock(Sprite blockSprite)
    {
        imgHold.gameObject.SetActive(true);

        imgHold.sprite = blockSprite;
    }

    public void SetImgNextBlocks(BlockDataSO.BlockData[] blockDatas)
    {
        for (int i = 0; i < imgNextBlocks.Length; i++)
        {
            imgNextBlocks[i].sprite = blockDatas[i].sprite;

            imgNextBlocks[i].DOFade(1f, 0f);
        }
    }

    private Sprite GetLogoSprite(LogoType logoType)
    {
        return logoDatasList.Find(x => x.logoType == logoType).sprLogo;
    }

    public void PrepareCheck()
    {
        StartCoroutine(CheckImagesDirection());
    }

    private IEnumerator CheckImagesDirection()
    {
        while (true)
        {
            float angleY = cameraTran.position.z < 0f ? 0f : 180f;

            imgHold.transform.eulerAngles = new Vector3(0f, angleY, 0f);

            for (int i = 0; i < imgNextBlocks.Length; i++)
            {
                imgNextBlocks[i].transform.eulerAngles = new Vector3(0f, angleY, 0f);
            }

            yield return null;
        }
    }

    public void ClearImgHoldBlock()
    {
        imgHold.sprite = null;

        imgHold.gameObject.SetActive(false);
    }

    public void SetTxtTimeLimitColor(Color color)
    {
        txtTimeLimit.color = color;
    }
}