using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using TNRD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LightingDemonstration
{
    public class UiManagerBase : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup cgRoot;

        [SerializeField]
        private bool fadeInOnLoadedScene;

        [SerializeField]
        private bool unloadOtherScenesOnInitialized = true;

        [SerializeField]
        private List<SerializableInterface<ISetup>> classesToSetup = new();

        protected CanvasGroup CgRoot { get => cgRoot; }

        private async void Start()
        {
            if (unloadOtherScenesOnInitialized) await MemoryUtility.UnloadAllScenesExceptCurrentSceneAsync();

            foreach (SerializableInterface<ISetup> classToSetup in classesToSetup)
            {
                if (classToSetup == null || classToSetup.Value == null)
                {
                    Debug.LogError("There is a null value in the \"" + nameof(classesToSetup) + "\".");

                    continue;
                }

                classToSetup.Value.Setup();
            }

            if (fadeInOnLoadedScene) await FadeInAsync();

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {

        }

        public void LoadSceneWithFadeOut(string sceneNameToLoad)
        {
            if (cgRoot == null)
            {
                Debug.LogError("The fade out animation could not be started because of a failure to get the \"" + nameof(cgRoot) + "\".");

                return;
            }

            cgRoot.DOFade(0f, ConstDataSO.Instance.UI_AnimationTime)
                .OnComplete(() => SceneManager.LoadScene(sceneNameToLoad));
        }

        protected async UniTask FadeInAsync()
        {
            if (cgRoot == null)
            {
                Debug.LogError("The fade in animation could not be started because of a failure to get the \"" + nameof(cgRoot) + "\".");

                return;
            }

            bool completed = false;

            cgRoot.interactable = false;

            cgRoot.alpha = 0f;

            cgRoot.DOFade(1f, ConstDataSO.Instance.UI_AnimationTime)
                .OnComplete(() =>
                {
                    cgRoot.interactable = true;

                    completed = true;
                });

            await UniTask.WaitUntil(() => completed);
        }
    }
}