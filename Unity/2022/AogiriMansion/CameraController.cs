using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Range(0.1f, 10f), Header("Š´“x")]
	public float lookSensitivity;

	[Range(0.1f, 1f), Header("ŠŠ‚ç‚©‚³")]
	public float lookSmooth;

	private float yRot;

	private float xRot;

	private float currentYRot;

	private float currentXRot;

	private float yRotVelocity;

	private float xRotVelocity;

	void Update()
	{
		yRot += Input.GetAxis("Mouse X") * lookSensitivity;

		xRot -= Input.GetAxis("Mouse Y") * lookSensitivity;

		currentXRot = Mathf.SmoothDamp(currentXRot, xRot, ref xRotVelocity, lookSmooth);

		currentYRot = Mathf.SmoothDamp(currentYRot, yRot, ref yRotVelocity, lookSmooth);

		transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);
	}
}