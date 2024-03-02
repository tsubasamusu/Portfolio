using DG.Tweening;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class UiManager_MakeSaveData : UiManagerBase
    {
        [SerializeField]
        private CanvasGroup cgMakeSaveData;

        [SerializeField]
        private TMP_InputField ifSaveDataName;

        [SerializeField]
        private TMP_InputField ifPasscode;

        [SerializeField]
        private TextMeshProUGUI tmpError_SaveDataName;

        [SerializeField]
        private TextMeshProUGUI tmpError_Passcode;

        [SerializeField]
        private ButtonController makeSaveDataButtonController;

        [SerializeField]
        private CgLoadingController cgLoadingController;

        protected override void OnLoadedClass()
        {
            ifSaveDataName.onValueChanged.AddListener(enteredText => OnEnteredSaveDataName(enteredText));

            ifPasscode.onValueChanged.AddListener(enteredText => OnEnteredPasscode(enteredText));

            tmpError_SaveDataName.text = tmpError_Passcode.text = string.Empty;

            cgMakeSaveData.interactable = false;

            cgMakeSaveData.alpha = 0f;

            cgMakeSaveData.DOFade(1f, ConstData.TIME_FADE)
                .OnComplete(() => cgMakeSaveData.interactable = true);
        }

        public void OnEnteredSaveDataName(string enteredSaveDataName)
        {
            if (enteredSaveDataName.Length > ConstData.MAX_LENGTH_SAVE_DATA_NAME)
            {
                ifSaveDataName.text = enteredSaveDataName[..ConstData.MAX_LENGTH_SAVE_DATA_NAME];

                return;
            }

            if (!string.IsNullOrEmpty(tmpError_SaveDataName.text)) tmpError_SaveDataName.text = string.Empty;

            ifSaveDataName.text = DataBaseManager.ConvertAvailableText(enteredSaveDataName);
        }

        public void OnEnteredPasscode(string enteredPasscode)
        {
            if (enteredPasscode.Length > ConstData.MAX_LENGTH_PASSCODE)
            {
                ifPasscode.text = enteredPasscode[..ConstData.MAX_LENGTH_PASSCODE];

                return;
            }

            if (!string.IsNullOrEmpty(tmpError_Passcode.text)) tmpError_Passcode.text = string.Empty;

            ifPasscode.text = DataBaseManager.ConvertAvailableText(enteredPasscode);
        }

        public async void OnClickedBtnMakeSaveData()
        {
            if (string.IsNullOrEmpty(ifSaveDataName.text))
            {
                tmpError_SaveDataName.text = ConstData.ERROR_UNCORRECT_SAVE_DATA_NAME;

                makeSaveDataButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            if (string.IsNullOrEmpty(ifPasscode.text))
            {
                tmpError_Passcode.text = ConstData.ERROR_UNCORRECT_PASSCODE;

                makeSaveDataButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            cgLoadingController.StartLoadingAnimation();

            cgMakeSaveData.interactable = false;

            makeSaveDataButtonController.ButtonText = ConstData.BUTTON_TEXT_SAVING;

            SaveData saveData = new()
            {
                saveDataName = ifSaveDataName.text,
                passcode = ifPasscode.text,
                members = GameData.Instance.LoginedData.members
            };

            MakeSaveDataResult makeSaveDataResult = await DataBaseManager.TryMakeSaveDataAsync(saveData);

            cgLoadingController.StopLoadingAnimation();

            if (makeSaveDataResult == MakeSaveDataResult.Success)
            {
                makeSaveDataButtonController.ButtonText = ConstData.BUTTON_TEXT_SUCCESS;

                cgMakeSaveData.DOFade(0f, ConstData.TIME_FADE)
                    .OnComplete(() => SceneManager.LoadScene(ConstData.SCENE_NAME_TITLE));

                return;
            }

            if (makeSaveDataResult == MakeSaveDataResult.Error)
            {
                makeSaveDataButtonController.ButtonText = ConstData.BUTTON_TEXT_ERROR;

                return;
            }

            tmpError_SaveDataName.text = ConstData.ERROR_ALREADY_EXIST_SAVE_DATA_NAME;

            makeSaveDataButtonController.ButtonText = ConstData.BUTTON_TEXT_MAKE_SAVE_DATA;

            makeSaveDataButtonController.ChangeButtonColorScheme();

            cgMakeSaveData.interactable = true;
        }
    }
}