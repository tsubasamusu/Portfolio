using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SchoolMetaverse
{
    public class CameraController : MonoBehaviourPunCallbacks, ISetUp
    {
        public void SetUp()
        {
            if (!photonView.IsMine) return;

            Vector3 mousePos = Vector3.zero;

            Vector3 cameraAngle = Vector3.zero;

            float yRotVelocity = 0f;

            float xRotVelocity = 0f;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    mousePos.y += Input.GetAxis("Mouse X") * GameData.instance.lookSensitivity;

                    mousePos.x -= Input.GetAxis("Mouse Y") * GameData.instance.lookSensitivity;

                    cameraAngle.x = Mathf.SmoothDamp(cameraAngle.x, mousePos.x, ref xRotVelocity, ConstData.LOOK_SMOOTH);

                    cameraAngle.y = Mathf.SmoothDamp(cameraAngle.y, mousePos.y, ref yRotVelocity, ConstData.LOOK_SMOOTH);

                    cameraAngle.x = Mathf.Clamp(cameraAngle.x, -ConstData.MAX_CAMERA_ANGLE_X, ConstData.MAX_CAMERA_ANGLE_X);

                    transform.eulerAngles = new(cameraAngle.x, cameraAngle.y, 0f);
                })
                .AddTo(this);
        }
    }
}