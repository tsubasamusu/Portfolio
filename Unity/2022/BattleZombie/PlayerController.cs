using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FPS
{
	public enum PlayerState
	{
		Walking,
		Running,
		Idle,
		Crouching,
		Standing
	}

	[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
	public class PlayerController : MonoBehaviour
	{
		[Range(0.1f, 2f), SerializeField]
		private float walkSpeed = 1.5f;

		[Range(0.1f, 10f), SerializeField]
		private float runSpeed = 3.5f;

		[Range(0.1f, 10f), SerializeField]
		private float gravity = 9.8f;

		[Range(1f, 15f), SerializeField]
		private float jumpPower = 10f;

		[Range(0.1f, 2f), SerializeField, Header("ÇµÇ·Ç™ÇÒÇæÇ∆Ç´ÇÃîwÇÃçÇÇ≥")]
		private float crouchHeight = 1f;

		[Range(0.1f, 5f), SerializeField, Header("í èÌéûÇÃîwÇÃçÇÇ≥")]
		private float normalHeight = 2f;

		[SerializeField]
		private Ease jumpEase;

		[HideInInspector]
		public PlayerState currentPlayerState;

		private AudioSource footStepSource;

		[SerializeField]
		private AudioClip landingSound;

		[HideInInspector]
		public float footSoundDelay;

		[SerializeField]
		private KeyCode jumpKey;

		[SerializeField]
		private KeyCode runKey;

		[SerializeField]
		private KeyCode crouchKey;

		private CharacterController charaController;

		private GameObject FPSCamera;

		private Vector3 desiredMove;

		private Vector3 startPos;

		void Start()
		{
			FPSCamera = GameObject.Find("FPSCamera");

			charaController = GetComponent<CharacterController>();

			footStepSource = GetComponent<AudioSource>();

			startPos = transform.position;
		}

		void Update()
		{
			if (charaController.isGrounded)
			{
				Move();

				PlayerFootSound();
			}

			Respawn();

			Crouch();

			charaController.Move(Vector3.down * gravity * Time.fixedDeltaTime);
		}

		PlayerState Move()
		{
			float moveH = Input.GetAxis("Horizontal");

			float moveV = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(moveH, 0, moveV);

			if (movement.sqrMagnitude > 1)
			{
				movement.Normalize();
			}

			desiredMove = FPSCamera.transform.forward * movement.z + FPSCamera.transform.right * movement.x;

			if (Input.GetKeyDown(jumpKey))
			{
				transform.DOMove(new Vector3(transform.position.x, transform.position.y + jumpPower, transform.position.z), 0.5f)
					.SetEase(jumpEase).SetLoops(2, LoopType.Yoyo)
					.OnComplete(() => { AudioSource.PlayClipAtPoint(landingSound, Camera.main.transform.position); });
			}

			if (Input.GetKey(runKey))
			{
				charaController.Move(desiredMove * Time.fixedDeltaTime * runSpeed);

				return PlayerState.Running;
			}
			else
			{
				charaController.Move(desiredMove * Time.fixedDeltaTime * walkSpeed);

				if (desiredMove.sqrMagnitude >= 0.9f)
				{
					return PlayerState.Walking;
				}

				return PlayerState.Idle;
			}
		}

		public void Respawn()
		{
			if (transform.position.y <= -10f)
			{
				transform.position = startPos;
			}
		}

		public PlayerState Crouch()
		{
			if (Input.GetKey(crouchKey))
			{
				charaController.height = crouchHeight;

				return PlayerState.Crouching;
			}
			else
			{
				charaController.height = normalHeight;

				return PlayerState.Standing;
			}
		}

		public void PlayerFootSound()
		{
			if (Move() == PlayerState.Walking)
			{
				footStepSource.enabled = true;

				footStepSource.pitch = 0.5f;

				if (Crouch() == PlayerState.Crouching)
				{
					footStepSource.volume = 0.5f;
				}
				else
				{
					footStepSource.volume = 1.0f;
				}
			}
			else if (Move() == PlayerState.Running)
			{
				footStepSource.enabled = true;

				footStepSource.pitch = 1.0f;
			}
			else if (Move() == PlayerState.Idle)
			{
				footStepSource.enabled = false;
			}
		}
	}
}