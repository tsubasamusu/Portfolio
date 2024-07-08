using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SK.GyroscopeWebGL;
using UnityEngine;

namespace LightingDemonstration
{
    public class CameraController : MonoBehaviour, ISetup
    {
        TweenerCore<Quaternion, Quaternion, NoOptions> cameraTweenerCore;

        Quaternion defaultLocalQuaternion;

        public float CurrentCameraHeight
        {
            get => transform.localPosition.y * 100f;

            set => transform.localPosition = new(0f, value / 100f, 0f);
        }

        public void Setup() => defaultLocalQuaternion = transform.localRotation;

        public void OnGetGyroData(GyroscopeData gyroscopeData)
        {
            cameraTweenerCore.Kill();

            transform.localRotation = gyroscopeData.UnityRotation;
        }

        public void ResetCameraAngle() => transform.DOLocalRotate(new(defaultLocalQuaternion.eulerAngles.x, transform.localEulerAngles.y, defaultLocalQuaternion.eulerAngles.z), ConstDataSO.Instance.cameraAnimationTime);
    }
}