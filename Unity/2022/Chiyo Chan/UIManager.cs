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
    private Text txtTime;

    [SerializeField]
    private Text txtLength;

    [SerializeField]
    private Sprite sprGameStart;

    [SerializeField]
    private Sprite sprGameClear;

    [SerializeField]
    private Transform playerTran;

    [SerializeField]
    private Transform goalTran;

    [SerializeField]
    private Transform resultTran;

    private float timer;

    private bool stopUpdateText;

    public bool StopUpdateText
    {
        set
        {
            stopUpdateText = value;
        }
    }

    public void SetUpUI()
    {
        imgBackground.color = Color.white;

        txtLength.text = txtTime.text = string.Empty;
    }

    public IEnumerator PlayGameStart()
    {
        bool end = false;

        imgBackground.DOFade(1f, 0f);

        imgLogo.sprite = sprGameStart;

        imgLogo.DOFade(0f, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(imgLogo.DOFade(1f, 1f).SetLoops(2, LoopType.Yoyo));

        sequence.Append(imgBackground.DOFade(0f, 1f)).OnComplete(() => end = true);

        yield return new WaitUntil(() => end);
    }

    public IEnumerator PlayGameClear()
    {
        txtLength.enabled = false;

        imgLogo.sprite = sprGameClear;

        imgBackground.DOFade(1f, 1f);

        imgLogo.DOFade(1f, 1f);

        txtTime.transform.DOMove(resultTran.position, 1f);

        txtTime.DOColor(Color.blue, 1f);

        yield return new WaitForSeconds(2f);

        imgLogo.DOFade(0f, 1f);

        txtTime.DOFade(0f, 1f);

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator StartUpdateText()
    {
        while (true)
        {
            if (stopUpdateText)
            {
                break;
            }

            txtLength.text = (goalTran.position.z - playerTran.position.z).ToString("F2") + "m\nTo Goal";

            timer += Time.deltaTime;

            txtTime.text = timer.ToString("F2") + "\nSecond";

            yield return null;
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }
}