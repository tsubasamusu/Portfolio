using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class UiManager_Title : UiManagerBase
    {
        [SerializeField]
        private CanvasGroup cgTitle;

        [SerializeField]
        private CgLoadingController cgLoadingController;

        protected override void OnLoadedClass()
        {
            cgTitle.interactable = false;

            cgTitle.alpha = 0f;

            cgTitle.DOFade(1f, ConstData.TIME_FADE).OnComplete(() => Setup());
        }

        private async void Setup()
        {
            cgLoadingController.StartLoadingAnimation();

            await CloudStorageData.Instance.SetAllSpritesFromCloudStorageAsync();

            if (!GameData.Instance.Logined)
            {
                cgLoadingController.StopLoadingAnimation();

                cgTitle.interactable = true;

                return;
            }

            bool successed = await DataBaseManager.TryUpdateLoginedDataAsync();

            cgLoadingController.StopLoadingAnimation();

            if (successed)
            {
                cgTitle.interactable = true;

                return;
            }

            GameData.Instance.LoginedData = null;

            GameData.Instance.DeleteStoredSaveData();
        }

        public void OnClickedBtnPlay() => cgTitle.DOFade(0f, ConstData.TIME_FADE).OnComplete(() => SceneManager.LoadScene(GameData.Instance.Logined ? ConstData.SCENE_NAME_SET_MAMBERS : ConstData.SCENE_NAME_LOGIN));
    }
}