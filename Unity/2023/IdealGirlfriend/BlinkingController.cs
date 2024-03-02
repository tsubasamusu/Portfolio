using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlinkingController : MonoBehaviour
{
    private SkinnedMeshRenderer smdFace;

    private void Start()
    {
        smdFace = GetComponent<SkinnedMeshRenderer>();

        StartBlinkingAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid StartBlinkingAsync(CancellationToken token)
    {
        while (true)
        {
            float blinkingSpan = UnityEngine.Random.Range(ConstData.MINI_BLINKING_INTERVAL, ConstData.MAX_BLINKING_INTERVAL);

            await UniTask.Delay(TimeSpan.FromSeconds(blinkingSpan), cancellationToken: token);

            BlinkAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    private async UniTaskVoid BlinkAsync(CancellationToken token)
    {
        float passedSeconds = 0f;

        float secondsRequiredToClose = 0f;

        bool isToClose = true;

        while (true)
        {
            passedSeconds += ConstData.BLINKING_DELTA_TIME;

            if (isToClose)
            {
                float closeValue = 100f * (passedSeconds / (ConstData.BLINKING_SECONDS / 2));

                SetEyeBlendShapeValue(closeValue);

                if (!IsCompletelyClosingEyes())
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(ConstData.BLINKING_DELTA_TIME), cancellationToken: token); ;

                    continue;
                }

                isToClose = false;
            }

            if (secondsRequiredToClose == 0f) secondsRequiredToClose = passedSeconds;

            float openValue = 100f - (100f * ((passedSeconds - secondsRequiredToClose) / (ConstData.BLINKING_SECONDS / 2)));

            SetEyeBlendShapeValue(openValue);

            if (!IsCompletelyOpeningEyes())
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ConstData.BLINKING_DELTA_TIME), cancellationToken: token);

                continue;
            }

            break;
        }
    }

    private bool IsCompletelyClosingEyes() => smdFace.GetBlendShapeWeight(ConstData.EYE_BLEND_SHAPE_INDEX) >= 100f;

    private bool IsCompletelyOpeningEyes() => smdFace.GetBlendShapeWeight(ConstData.EYE_BLEND_SHAPE_INDEX) <= 0f;

    private void SetEyeBlendShapeValue(float setValue)
    {
        float value = Mathf.Clamp(setValue, 0f, 100f);

        smdFace.SetBlendShapeWeight(ConstData.EYE_BLEND_SHAPE_INDEX, value);
    }
}