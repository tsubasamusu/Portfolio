using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using FPS;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image imgEventHorizon;

    [SerializeField]
    private float firstPlayerHp;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private Slider playerHpSlider;

    [SerializeField]
    private float grenadeDamage;

    public float damageRange;

    [SerializeField, Header("1ïbÇ†ÇΩÇËÇÃHPå∏è≠ó ")]
    private float everySecondReduceTheAmount;

    [HideInInspector]
    private float currentPlayerHp;

    void Start()
    {
        currentPlayerHp = firstPlayerHp;
    }

    void Update()
    {
        CaluculateCurrentPlayerHp(CheckThatEnemyisDamageRange());
    }

    public bool CheckThatEnemyisDamageRange()
    {
        if (enemyGenerator.enemyList.Count == 0)
        {
            return false;
        }

        float nearLength = (enemyGenerator.enemyList[0].transform.position - transform.position).magnitude;//Åu0Åvî‘ñ⁄Ç1î‘ãﬂÇ¢ìGÇ∆ÇµÇƒâºìoò^Çµãóó£ÇåvéZ

        for (int i = 0; i < enemyGenerator.enemyList.Count; i++)
        {
            float length = (enemyGenerator.enemyList[i].transform.position - transform.position).magnitude;

            if (length < nearLength)
            {
                nearLength = length;
            }
        }

        if (nearLength <= damageRange)
        {
            cameraController.ShakeCamera();

            return true;
        }

        return false;
    }

    private void CaluculateCurrentPlayerHp(bool enemyIsDamageRange)
    {
        if (enemyIsDamageRange)
        {
            DOTween.To(() => currentPlayerHp, (x) => currentPlayerHp = x, currentPlayerHp - (everySecondReduceTheAmount / 10f), 0.1f);

            ChangeImgEventHorizonAlpha();

            ChangePlayerHpSlider();
        }

        if (currentPlayerHp <= 0)
        {
            SceneManager.LoadScene("Title");
        }
    }

    private void ChangeImgEventHorizonAlpha()
    {
        imgEventHorizon.color = new Color(255, 0, 0, 0.3f - 0.3f * (currentPlayerHp / firstPlayerHp));
    }

    private void ChangePlayerHpSlider()
    {
        playerHpSlider.value = currentPlayerHp / firstPlayerHp;
    }

    public void AttackedByGrenade()
    {
        currentPlayerHp -= grenadeDamage;

        ChangePlayerHpSlider();
    }
}