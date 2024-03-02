using UnityEngine;

namespace AogiriRoom
{
    public class ViewCameraController : MonoBehaviour
    {
        private readonly KeyCode rotateCameraKey = KeyCode.Mouse0;

        private readonly float moveValueForMouse = 10f;

        private readonly float moveValueForTouch = 50f;

        private readonly float lookSensitivityForMouse = 100f;

        private readonly float lookSensitivityForTouch = 1f;

        private float rotationX;

        private float rotationY;

        public void Update()
        {
            if (Input.touchSupported && Input.touchCount >= 1)
            {
                ControlMovementForTouch();

                ControlRotationForTouch();

                return;
            }

            ControlMovementForMouse();

            ControlRotationForMouse();
        }

        private void ControlMovementForMouse()
        {
            float scrolledValue = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * 10f, -1f, 1f);

            Vector3 moveDir = moveValueForMouse * scrolledValue * Time.deltaTime * transform.forward;

            transform.position = transform.position + moveDir;
        }

        private void ControlRotationForMouse()
        {
            if (!Input.GetKey(rotateCameraKey)) return;

            rotationY += Input.GetAxis("Mouse X") * lookSensitivityForMouse * Time.deltaTime;

            rotationX -= Input.GetAxis("Mouse Y") * lookSensitivityForMouse * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        private void ControlMovementForTouch()
        {
            if (!Input.touchSupported) return;

            if (Input.touchCount != 2) return;

            Touch touch1 = Input.GetTouch(0);

            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase != TouchPhase.Moved && touch2.phase != TouchPhase.Moved) return;

            Vector2 beforePos1 = touch1.position - touch1.deltaPosition;

            Vector2 beforePos2 = touch2.position - touch2.deltaPosition;

            float currentLength = (touch1.position - touch2.position).magnitude;

            float beforeLength = (beforePos1 - beforePos2).magnitude;

            float deltaLength = currentLength - beforeLength;

            Vector3 moveDir = moveValueForTouch * deltaLength * Time.deltaTime * transform.forward;

            transform.position = transform.position + moveDir;
        }

        private void ControlRotationForTouch()
        {
            if (!Input.touchSupported) return;

            if (Input.touchCount != 1) return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Moved) return;

            rotationY += touch.deltaPosition.x * lookSensitivityForTouch * Time.deltaTime;

            rotationX -= touch.deltaPosition.y * lookSensitivityForTouch * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}