using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float gravity;

	[SerializeField]
	private KeyCode flashlightKey;

	[SerializeField]
	private KeyCode restartKey;

	[SerializeField]
	private GameObject flashlight;

	[SerializeField]
	private Transform mainCamera;

	[SerializeField]
	private CharacterController charaController;

	[SerializeField]
	private EventManager eventManager;

	[SerializeField]
	private SoundManager soundManager;

	[SerializeField]
	private UIManager uiManager;

	private Vector3 desiredMove;

	private bool flashlightIsActive;

	private void Start()
	{
		SetUpFlashlight();
	}

	private void Update()
	{
		SetMovement();

		SetFlashlight();
	}

	private void FixedUpdate()
	{
		charaController.Move(desiredMove * Time.fixedDeltaTime * moveSpeed);

		charaController.Move(-transform.up * gravity * Time.fixedDeltaTime);
	}

	private void SetMovement()
	{
		float moveH = Input.GetAxis("Horizontal");

		float moveV = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveH, 0, moveV);

		desiredMove = mainCamera.forward * movement.z + mainCamera.right * movement.x;

		if (movement.magnitude > 0)
		{
			soundManager.SetPlayerAudioSourse(true);
		}
		else
		{
			soundManager.SetPlayerAudioSourse(false);
		}
	}

	private void SetUpFlashlight()
	{
		flashlight.SetActive(true);

		flashlightIsActive = true;
	}

	private void SetFlashlight()
	{
		if (!Input.GetKeyDown(flashlightKey))
		{
			return;
		}

		flashlight.SetActive(!flashlightIsActive);

		flashlightIsActive = flashlightIsActive ? false : true;
	}

	private IEnumerator CheckRestart()
	{
		while (true)
		{
			if (Input.GetKeyDown(restartKey))
			{
				SceneManager.LoadScene("Main");
			}

			yield return null;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!hit.gameObject.TryGetComponent(out EventDetail eventDetail))
		{
			return;
		}

		eventDetail.TriggerEvent();

		PlayEvents(hit);

		if ((eventDetail.CharaEvent == null))
		{
			soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NormalHorrorSE));
		}
	}

	private void PlayEvents(ControllerColliderHit hit)
	{
		GameData.instance.MetMembersCount++;

		Destroy(hit.gameObject);

		uiManager.UpdateTxtMembersCount();

		if (GameData.instance.MetMembersCount == 8)
		{
			uiManager.UpdateTxtMessage("Tap " + "'" + restartKey.ToString() + "'" + "\nTo\nRestart", Color.red);

			StartCoroutine(CheckRestart());
		}
	}
}