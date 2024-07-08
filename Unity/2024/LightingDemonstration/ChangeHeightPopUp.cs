using TMPro;
using UniRx;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace LightingDemonstration
{
    public class ChangeHeightPopUp : MonoBehaviour, ISetup
    {
        [SerializeField]
        private CanvasGroup cgChangeHeightPopUp;

        [SerializeField]
        private CanvasGroup cgButtons;

        [SerializeField]
        private Slider changeHeightSlider;

        [SerializeField]
        private TextMeshProUGUI tmpCurrentHeight;

        [SerializeField]
        private Joystick joystick;

        [SerializeField]
        private ButtonsParent buttonsParent;

        [SerializeField]
        private CameraController cameraController;

        public void Setup()
        {
            ChangeSliderValueByCurrentCameraHeight();

            changeHeightSlider.OnValueChangedAsObservable()
                .Subscribe(newValue => OnSliderValueChanged(newValue))
                .AddTo(this);

            HidePopUp();
        }

        public void ShowPopUp()
        {
            cgChangeHeightPopUp.alpha = 1f;

            cgChangeHeightPopUp.blocksRaycasts = true;

            cgChangeHeightPopUp.interactable = true;

            buttonsParent.SetEnable(false, true);

            joystick.SetEnable(false, true);
        }

        public void HidePopUp()
        {
            cgChangeHeightPopUp.alpha = 0f;

            cgChangeHeightPopUp.blocksRaycasts = false;

            cgChangeHeightPopUp.interactable = false;

            buttonsParent.SetEnable(true, true);

            joystick.SetEnable(true, true);
        }

        private void OnSliderValueChanged(float newValue)
        {
            cameraController.CurrentCameraHeight = ConstDataSO.Instance.minCameraHeight + ((ConstDataSO.Instance.maxCameraHeight - ConstDataSO.Instance.minCameraHeight) * newValue);

            UpdateCurrentHeightText();
        }

        private void ChangeSliderValueByCurrentCameraHeight()
        {
            changeHeightSlider.value = (cameraController.CurrentCameraHeight - ConstDataSO.Instance.minCameraHeight) / (ConstDataSO.Instance.maxCameraHeight - ConstDataSO.Instance.minCameraHeight);

            UpdateCurrentHeightText();
        }

        private void UpdateCurrentHeightText() => tmpCurrentHeight.text = Mathf.Floor(cameraController.CurrentCameraHeight).ToString();
    }
}