using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LightingDemonstration
{
    public class MemoryUtility
    {
        public static async UniTask UnloadAllScenesExceptCurrentSceneAsync()
        {
            if (SceneManager.loadedSceneCount == 1) return;

            string currentSceneName = SceneManager.GetActiveScene().name;

            foreach (SceneData sceneData in ConstDataSO.Instance.sceneDatas)
            {
                if (sceneData.sceneName == currentSceneName) continue;

                try
                {
                    await SceneManager.UnloadSceneAsync(sceneData.sceneName);
                }
                catch (Exception exception)
                {
                    if (exception is ArgumentException) continue;

                    Debug.LogException(exception);

                    continue;
                }
            }
        }

        public static void DisposeDownloadedHouseData()
        {
            if (GameData.Instance.reservedHouseData.IsValid()) GameData.Instance.reservedHouseData.downloadedHouseData.Dispose();
        }
    }
}