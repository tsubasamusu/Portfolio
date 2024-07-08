using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace LightingDemonstration
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, ISetup
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Transform cameraTransform;

        public void Setup()
        {
            //Player Transform
            {
                characterController.enabled = false;

                transform.position = GameData.Instance.reservedHouseData.playerFirstPosition;

                transform.eulerAngles = GameData.Instance.reservedHouseData.playerFirstAngles;

                characterController.enabled = true;
            }

            this.UpdateAsObservable()
                .Where(_ => !characterController.isGrounded)
                .Subscribe(_ => characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime))
                .AddTo(this);
        }

        public void OnInputJoystick(Vector2 inputValue)
        {
            Move(inputValue);

            Rotate(inputValue);
        }

        private void Move(Vector2 inputValue)
        {
            if (characterController == null) return;

            Vector3 moveValue = (cameraTransform.forward * inputValue.y + transform.right * inputValue.x * Mathf.Abs(inputValue.y)) * ConstDataSO.Instance.playerMoveSpeedPerSecond * Time.deltaTime;

            characterController.Move(moveValue);
        }

        private void Rotate(Vector2 inputValue)
        {
            float rotateAngle = inputValue.x * ConstDataSO.Instance.playerRotateAnglePerSecond * Time.deltaTime;

            transform.Rotate(new(0f, rotateAngle, 0f));
        }
    }
}