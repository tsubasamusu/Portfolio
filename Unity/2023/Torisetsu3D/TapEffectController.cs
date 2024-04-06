using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TapEffectController : MonoBehaviour
{
    [SerializeField, Range(0f, 0.01f)]
    private float timeBtweenSprites;

    [SerializeField]
    private Image imgTapEffect;

    [SerializeField]
    private List<Sprite> tapEffectSprites = new();

    public void SetUpTapEffectController(Transform canvasTran, Vector3 pos)
    {
        transform.SetParent(canvasTran);

        transform.localPosition = pos;

        PlayTapEffectAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid PlayTapEffectAsync(CancellationToken token)
    {
        for (int i = 0; i < tapEffectSprites.Count; i++)
        {
            imgTapEffect.sprite = tapEffectSprites[i];

            await UniTask.Delay(TimeSpan.FromSeconds(timeBtweenSprites), cancellationToken: token);
        }

        for (int i = 0; i < tapEffectSprites.Count; i++)
        {
            imgTapEffect.sprite = tapEffectSprites[(tapEffectSprites.Count - 1) - i];

            await UniTask.Delay(TimeSpan.FromSeconds(timeBtweenSprites), cancellationToken: token);
        }

        Destroy(gameObject);
    }
}