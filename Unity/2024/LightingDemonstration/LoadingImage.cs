using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LightingDemonstration
{
    [RequireComponent(typeof(Image))]
    public class LoadingImage : MonoBehaviour, ISetup
    {
        private CancellationTokenSource cancellationTokenSource;

        public void Setup()
        {
            SceneManager.sceneUnloaded += OnUnloadedScene;

            SetImageEnabled(false);
        }

        private void OnUnloadedScene(Scene unloadedScene) => StopLoadingAnimation();

        public void StartLoadingAnimation()
        {
            SetImageEnabled(true);

            cancellationTokenSource = new();

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            StartLoadingAnimationAsync(cancellationToken).Forget();
        }

        private async UniTaskVoid StartLoadingAnimationAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ConstDataSO.Instance.loadingRotateSpan), cancellationToken: cancellationToken);

                transform.Rotate(new(0f, 0f, -ConstDataSO.Instance.loadingRotateAngle));
            }
        }

        private void SetImageEnabled(bool enabled)
        {
            if (!TryGetComponent(out Image image))
            {
                Debug.LogError("Failed to get the Image.");

                return;
            }

            image.enabled = enabled;
        }

        public void StopLoadingAnimation() => cancellationTokenSource?.Cancel();
    }
}