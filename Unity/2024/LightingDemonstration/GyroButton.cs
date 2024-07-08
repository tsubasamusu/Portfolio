using SK.GyroscopeWebGL;
using UnityEngine;
using UnityEngine.UI;

namespace LightingDemonstration
{
    public class GyroButton : ButtonBase
    {
        [SerializeField]
        private CameraController cameraController;

        [SerializeField]
        private Image imgGyroButton;

        protected override void OnInitialized()
        {
            if (SK_DeviceSensor.IsGyroscopeStarted) SK_DeviceSensor.StopGyroscopeListener();
        }

        protected override void OnClickedButton()
        {
            imgGyroButton.sprite = SK_DeviceSensor.IsGyroscopeStarted ? ConstDataSO.Instance.gyroButtonDefaultSprite : ConstDataSO.Instance.gyroButtonActiveSprite;

            if (SK_DeviceSensor.IsGyroscopeStarted)
            {
                SK_DeviceSensor.StopGyroscopeListener();

                cameraController.ResetCameraAngle();

                return;
            }

            SK_DeviceSensor.StartGyroscopeListener(cameraController.OnGetGyroData);
        }
    }
}