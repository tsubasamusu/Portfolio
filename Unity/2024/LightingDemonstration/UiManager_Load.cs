using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LightingDemonstration
{
    public class UiManager_Load : UiManagerBase
    {
        [SerializeField]
        private LoadingImage loadingImage;

        protected override void OnInitialized()
        {
            loadingImage.StartLoadingAnimation();

            UnityEvent onDownloadedHouseDataEvent = new();

            onDownloadedHouseDataEvent.AddListener(() =>
            {
                loadingImage.StopLoadingAnimation();

                SceneManager.LoadScene(ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Preview));
            });

            GameData.Instance.DownloadAssetsForReservedHouseAsync(onDownloadedHouseDataEvent).Forget();
        }
    }
}