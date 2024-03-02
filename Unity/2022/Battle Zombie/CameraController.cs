using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FPS
{
	public class CameraController : MonoBehaviour
	{
		[Range(0.1f, 10f), SerializeField, Header("ä¥ìx")]
		private float lookSensitivity = 5f;

		[Range(0.1f, 1f), SerializeField, Header("ääÇÁÇ©Ç≥")]
		private float lookSmooth = 0.1f;

		[SerializeField, Header("äpìxêßå¿")]
		private Vector2 MinMaxAngle = new Vector2(-65, 65);

		[SerializeField]
		private PlayerHealth playerHealth;

		private float yRot;

		private float xRot;

		private float currentYRot;

		private float currentXRot;

		private float yRotVelocity;

		private float xRotVelocity;

		private Vector3 localPos;

		private void Start()
		{
			localPos = transform.localPosition;
		}

		void Update()
		{
			ControlCamera();

			transform.localPosition = new Vector3(transform.localPosition.x, localPos.y, transform.localPosition.z);
		}

		private void ControlCamera()
		{
			yRot += Input.GetAxis("Mouse X") * lookSensitivity;

			xRot -= Input.GetAxis("Mouse Y") * lookSensitivity;

			xRot = Mathf.Clamp(xRot, MinMaxAngle.x, MinMaxAngle.y);

			currentXRot = Mathf.SmoothDamp(currentXRot, xRot, ref xRotVelocity, lookSmooth);

			currentYRot = Mathf.SmoothDamp(currentYRot, yRot, ref yRotVelocity, lookSmooth);

			transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);
		}

		public void ShakeCamera()
		{
			transform.DOShakePosition(1.0f, 0.5f, 3, 1, false, true);
		}
	}
}