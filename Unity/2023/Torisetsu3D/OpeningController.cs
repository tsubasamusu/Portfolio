using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class OpeningController : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime;

    [SerializeField, Range(0f, 0.1f)]
    private float timeBtweenSprites;

    [SerializeField]
    private Image imgBackground;

    [SerializeField]
    private Image imgInformation;

    [SerializeField]
    private AnimationController animationController;

    [SerializeField]
    private ButtonColorChanger btnStart;

    [SerializeField]
    private List<Sprite> informationSprites = new();

    private void Start()
    {
        StartOpeningAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid StartOpeningAsync(CancellationToken token)
    {
        for (int i = 0; i < informationSprites.Count; i++)
        {
            imgInformation.sprite = informationSprites[i];

            await UniTask.Delay(TimeSpan.FromSeconds(timeBtweenSprites), cancellationToken: token);
        }

        imgBackground.DOFade(0f, fadeOutTime)
            .OnComplete(() => Destroy(imgBackground.gameObject));

        imgInformation.DOFade(0f, fadeOutTime)
            .OnComplete(() => Destroy(imgInformation.gameObject));

        await UniTask.Delay(TimeSpan.FromSeconds(fadeOutTime), cancellationToken: token);

        animationController.StartAnimation();

        btnStart.OnClicked();

        Destroy(gameObject);
    }
}