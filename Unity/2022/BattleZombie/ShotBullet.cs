using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShotBullet : MonoBehaviour
{
	[SerializeField]
	private Rigidbody bulletPrefab;

	[SerializeField]
	private GameObject shotEffect;

	[SerializeField]
	private Camera fpsCamera;

	[SerializeField]
	private AudioClip shotSound;

	[SerializeField]
	private AudioClip reloadSound;

	[SerializeField]
	private AudioClip outOfBulletSound;

	[SerializeField]
	private float shotSpeed;

	[SerializeField]
	private KeyCode reloadKey;

	[SerializeField]
	private float shotBulletSpan;

	[SerializeField]
	private Text txtBulletCount;

	[SerializeField, Header("‘•“U‰Â”\’e”")]
	private int firstShotCount;

	private int shotCount;

	private float firstFpsCameraFieldOfView;

	private float timer;

	private void Start()
	{
		shotCount = firstShotCount;

		UpdateBulletCount();

		firstFpsCameraFieldOfView = fpsCamera.fieldOfView;
	}

	void Update()
	{
		ControlShootingBullet();
	}

	private void ControlShootingBullet()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			timer += Time.deltaTime;

			if (shotCount > 0 && timer >= shotBulletSpan)
			{
				fpsCamera.DOFieldOfView(20f, 0.5f);

				shotCount -= 1;

				UpdateBulletCount();

				Rigidbody bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

				bullet.AddForce(transform.forward * shotSpeed);

				Destroy(bullet, 3.0f);

				AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position);

				GameObject effect = Instantiate(shotEffect, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

				effect.transform.position = transform.position;

				Destroy(effect, 0.1f);

				timer = 0;
			}
			else if (shotCount <= 0)
			{
				AudioSource.PlayClipAtPoint(outOfBulletSound, Camera.main.transform.position);
			}
		}
		else if (Input.GetKeyDown(reloadKey))
		{
			shotCount = firstShotCount;

			UpdateBulletCount();

			AudioSource.PlayClipAtPoint(reloadSound, Camera.main.transform.position);
		}
		else
		{
			fpsCamera.DOFieldOfView(firstFpsCameraFieldOfView, 0.5f);
		}
	}

	private void UpdateBulletCount()
	{
		txtBulletCount.text = shotCount.ToString();
	}
}