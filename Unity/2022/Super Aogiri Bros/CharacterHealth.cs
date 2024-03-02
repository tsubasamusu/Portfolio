using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tsubasa;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform parentTran;

    [SerializeField]
    private GameObject enemy;

    private GameManager gameManager;

    private CameraController cameraController;

    private float damage = 0f;

    private void Update()
    {
        if (CheckGameRange())
        {
            return;
        }

        if (enemy == null)
        {
            return;
        }

        KillMe();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackPoint"))
        {
            Attacked(other.transform);

            other.gameObject.SetActive(false);
        }
    }

    public void SetUpCharacterHealth(GameManager gameManager, CameraController cameraController)
    {
        this.gameManager = gameManager;

        this.cameraController = cameraController;
    }

    private void Attacked(Transform enemyTran)
    {
        damage += 10f;

        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Explosion).clip);

        if (enemyTran.position.x > transform.position.x)
        {
            transform.DOMoveX(transform.position.x - (damage * GameData.instance.powerRatio), 0.5f);
        }
        else if (enemyTran.position.x < transform.position.x)
        {
            transform.DOMoveX(transform.position.x + (damage * GameData.instance.powerRatio), 0.5f);
        }

        transform.DOMoveY(transform.position.y + (damage * GameData.instance.powerRatio), 0.5f);

        GameObject effect = Instantiate(GameData.instance.attackEffect, enemyTran.position, Quaternion.identity, parentTran);

        Destroy(effect, 1f);
    }

    public float GetDamage()
    {
        return damage;
    }

    private bool CheckGameRange()
    {
        if (transform.position.x <= 15f && transform.position.x >= -15f && transform.position.y <= 7f && transform.position.y >= -7f)
        {
            return true;
        }

        return false;
    }

    private void KillMe()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Dead).clip);

        GameObject effect = Instantiate(GameData.instance.deadEffect, transform.position, Quaternion.identity, parentTran);

        Destroy(effect, 1f);

        gameManager.SetUpEndGame();

        cameraController.targetTransList.Remove(transform);

        Destroy(gameObject);
    }
}