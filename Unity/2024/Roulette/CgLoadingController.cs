using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Roulette
{
    public class CgLoadingController : MonoBehaviour, ISetup
    {
        [SerializeField]
        private CanvasGroup cgLoading;

        private CancellationTokenSource cancellationTokenSource;

        public void Setup() => cgLoading.alpha = 0;

        public void StartLoadingAnimation()
        {
            cgLoading.alpha = 1;

            cancellationTokenSource = new();

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            StartLoadingAnimationAsync(cancellationToken).Forget();
        }

        private async UniTaskVoid StartLoadingAnimationAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ConstData.TIME_SPAN_ROTATE_LOADING), cancellationToken: cancellationToken);

                transform.Rotate(new(0f, 0f, -ConstData.ANGLE_ROTATE_LOADING));
            }
        }

        public void StopLoadingAnimation()
        {
            cancellationTokenSource?.Cancel();

            cgLoading.alpha = 0;
        }
    }
}