using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Roulette
{
    public class ErrorDisplayerController : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup cgErrorDisplayer;

        [SerializeField]
        private Image imgBackground;

        [SerializeField]
        private TextMeshProUGUI tmpError;

        [SerializeField]
        private ButtonController backToTitleButtonController;

        public static ErrorDisplayerController Instance
        {
            get;

            private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            ResetErrorDisplayer();

            backToTitleButtonController.Setup();
        }

        public void OnSceneLoaded(Scene nextScene, LoadSceneMode mode) => ResetErrorDisplayer();

        private void ResetErrorDisplayer()
        {
            cgErrorDisplayer.interactable = false;

            tmpError.text = string.Empty;

            cgErrorDisplayer.gameObject.SetActive(false);

            imgBackground.gameObject.SetActive(false);
        }

        public void DisplayError(string errorMessage)
        {
            cgErrorDisplayer.gameObject.SetActive(true);

            imgBackground.gameObject.SetActive(true);

            tmpError.text += "\n" + errorMessage;

            cgErrorDisplayer.interactable = true;
        }

        public void OnClickedBtnBackToTitle()
        {
            cgErrorDisplayer.interactable = false;

            cgErrorDisplayer.DOFade(0f, ConstData.TIME_FADE);

            imgBackground.DOColor(Color.white, ConstData.TIME_FADE)
                .OnComplete(() => SceneManager.LoadScene(ConstData.SCENE_NAME_TITLE));
        }
    }
}