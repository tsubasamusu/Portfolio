using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Roulette
{
    public class UiManager_Login : UiManagerBase
    {
        [SerializeField]
        private CanvasGroup cgLogin;

        [SerializeField]
        private TMP_InputField ifSaveDataName;

        [SerializeField]
        private TMP_InputField ifPasscode;

        [SerializeField]
        private TextMeshProUGUI tmpError_SaveDataName;

        [SerializeField]
        private TextMeshProUGUI tmpError_Passcode;

        [SerializeField]
        private Toggle tglSaveLoginInformation;

        [SerializeField]
        private ButtonController nextButtonController;

        [SerializeField]
        private CgLoadingController cgLoadingController;

        protected override void OnLoadedClass()
        {
            ifSaveDataName.onValueChanged.AddListener(enteredText => OnEnteredSaveDataName(enteredText));

            ifPasscode.onValueChanged.AddListener(enteredText => OnEnteredPasscode(enteredText));

            tglSaveLoginInformation.onValueChanged.AddListener(saveLoginInformation => GameData.Instance.saveLoginInformation = saveLoginInformation);

            tmpError_SaveDataName.text = tmpError_Passcode.text = string.Empty;

            cgLogin.interactable = false;

            cgLogin.alpha = 0f;

            cgLogin.DOFade(1f, ConstData.TIME_FADE).OnComplete(() =>
            {
                cgLogin.interactable = true;

                tglSaveLoginInformation.interactable = false;
            });
        }

        private void OnEnteredSaveDataName(string enteredSaveDataName)
        {
            if (enteredSaveDataName.Length > ConstData.MAX_LENGTH_SAVE_DATA_NAME)
            {
                ifSaveDataName.text = enteredSaveDataName[..ConstData.MAX_LENGTH_SAVE_DATA_NAME];

                return;
            }

            tglSaveLoginInformation.interactable = !string.IsNullOrEmpty(enteredSaveDataName) && !string.IsNullOrEmpty(ifPasscode.text);

            if (!string.IsNullOrEmpty(tmpError_SaveDataName.text)) tmpError_SaveDataName.text = string.Empty;

            ifSaveDataName.text = DataBaseManager.ConvertAvailableText(enteredSaveDataName);
        }

        private void OnEnteredPasscode(string enteredPasscode)
        {
            if (enteredPasscode.Length > ConstData.MAX_LENGTH_PASSCODE)
            {
                ifPasscode.text = enteredPasscode[..ConstData.MAX_LENGTH_PASSCODE];

                return;
            }

            tglSaveLoginInformation.interactable = !string.IsNullOrEmpty(ifSaveDataName.text) && !string.IsNullOrEmpty(enteredPasscode);

            if (!string.IsNullOrEmpty(tmpError_Passcode.text)) tmpError_Passcode.text = string.Empty;

            ifPasscode.text = DataBaseManager.ConvertAvailableText(enteredPasscode);
        }

        public async void OnClickedBtnNextAsync()
        {
            if (string.IsNullOrEmpty(ifSaveDataName.text) && string.IsNullOrEmpty(ifPasscode.text))
            {
                tmpError_SaveDataName.text = tmpError_Passcode.text = string.Empty;

                TransitionToEnterMamberScene();

                return;
            }

            if (string.IsNullOrEmpty(ifSaveDataName.text))
            {
                tmpError_SaveDataName.text = ConstData.ERROR_UNCORRECT_SAVE_DATA_NAME;

                nextButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            if (string.IsNullOrEmpty(ifPasscode.text))
            {
                tmpError_Passcode.text = ConstData.ERROR_UNCORRECT_PASSCODE;

                nextButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            cgLoadingController.StartLoadingAnimation();

            cgLogin.interactable = false;

            nextButtonController.ButtonText = ConstData.BUTTON_TEXT_CHECKING;

            LoginResult loginResult = await DataBaseManager.TryLoginAsync(ifSaveDataName.text, ifPasscode.text);

            cgLoadingController.StopLoadingAnimation();

            if (loginResult == LoginResult.Success)
            {
                nextButtonController.ButtonText = ConstData.BUTTON_TEXT_SUCCESS;

                TransitionToEnterMamberScene();

                return;
            }

            if (loginResult == LoginResult.Error)
            {
                nextButtonController.ButtonText = ConstData.BUTTON_TEXT_ERROR;

                return;
            }

            nextButtonController.ButtonText = ConstData.BUTTON_TEXT_NEXT;

            nextButtonController.ChangeButtonColorScheme();

            cgLogin.interactable = true;

            if (loginResult == LoginResult.EnteredNonExistentSaveDataName)
            {
                tmpError_SaveDataName.text = ConstData.ERROR_UNCORRECT_SAVE_DATA_NAME;

                return;
            }

            tmpError_Passcode.text = ConstData.ERROR_UNCORRECT_PASSCODE;
        }

        private void TransitionToEnterMamberScene()
        {
            if (!GameData.Instance.saveLoginInformation) GameData.Instance.DeleteStoredSaveData();

            cgLogin.interactable = false;

            cgLogin.DOFade(0f, ConstData.TIME_FADE)
                .OnComplete(() => SceneManager.LoadScene(ConstData.SCENE_NAME_SET_MAMBERS));
        }
    }
}