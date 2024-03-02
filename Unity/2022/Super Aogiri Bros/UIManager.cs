using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image imgBackground;

    [SerializeField]
    private Image imgLogo;

    [SerializeField]
    private Image imgPicture;

    [SerializeField]
    private Text txtMessage;

    [SerializeField]
    private Sprite sprTitle;

    [SerializeField]
    private Sprite sprModeSelect;

    [SerializeField]
    private Sprite sprCharaSelect;

    private float time = 3.5f;

    public IEnumerator PlayGameStart()
    {
        bool endGameStart = false;

        imgLogo.sprite = sprTitle;

        imgLogo.DOFade(0f, 0f);

        imgBackground.color = Color.white;

        imgBackground.DOFade(1f, 0f);

        imgLogo.DOFade(1f, 1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => { endGameStart = true; });

        yield return new WaitUntil(() => endGameStart);
    }

    public IEnumerator SetModeSelect()
    {
        bool end = false;

        imgPicture.sprite = sprModeSelect;

        imgPicture.DOFade(1f, 1f).OnComplete(() => end = true);

        yield return new WaitUntil(() => end);
    }

    public IEnumerator SetCharaSelect()
    {
        bool end = false;

        imgPicture.DOFade(0f, 0.5f)
            .OnComplete(() =>
            {
                imgPicture.sprite = sprCharaSelect;

                imgPicture.DOFade(1f, 0.5f).OnComplete(() => end = true);
            });

        yield return new WaitUntil(() => end);
    }

    public IEnumerator GoToGame()
    {
        bool end = false;

        imgPicture.DOFade(0f, 1f);

        imgBackground.DOFade(0f, 1f).OnComplete(() => end = true);

        yield return new WaitUntil(() => end);
    }

    public IEnumerator CountDown()
    {
        txtMessage.color = Color.green;

        txtMessage.DOFade(1f, 0f);

        while (true)
        {
            time -= Time.deltaTime;

            if (time <= 0.5f)
            {
                txtMessage.text = "GO";

                txtMessage.DOFade(0f, 1f).OnComplete(() => txtMessage.text = "");

                break;
            }

            txtMessage.text = time.ToString("F0");

            yield return null;
        }
    }

    public IEnumerator EndGame()
    {
        bool end = false;

        txtMessage.color = Color.blue;

        txtMessage.text = "Game Set";

        txtMessage.DOFade(1f, 0.5f).OnComplete(() => txtMessage.DOFade(0f, 1f).OnComplete(() => end = true));

        yield return new WaitUntil(() => end);
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }
}