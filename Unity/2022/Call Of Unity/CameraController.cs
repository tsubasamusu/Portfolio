using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace CallOfUnity
{
    public class CameraController : MonoBehaviour, ISetUp
    {
        private float yRot;

        private float xRot;

        private float currentYRot;

        private float currentXRot;

        private float yRotVelocity;

        private float xRotVelocity;

        public void SetUp()
        {
            Camera.main.fieldOfView = ConstData.NORMAL_FOV;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    yRot += Input.GetAxis("Mouse X") * GameData.instance.lookSensitivity;

                    xRot -= Input.GetAxis("Mouse Y") * GameData.instance.lookSensitivity;

                    currentXRot = Mathf.SmoothDamp(currentXRot, xRot, ref xRotVelocity, GameData.instance.lookSmooth);

                    currentYRot = Mathf.SmoothDamp(currentYRot, yRot, ref yRotVelocity, GameData.instance.lookSmooth);

                    transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);
                })
                .AddTo(this);
        }
    }
}