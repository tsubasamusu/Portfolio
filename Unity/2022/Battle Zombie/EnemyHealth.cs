using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float firstEnemyHp;

    [SerializeField, Header("1ïbÇ†ÇΩÇËÇÃHPå∏è≠ó ")]
    private float everySecondReduceTheAmount;

    [SerializeField]
    private Slider enemyHpSlider;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private AudioClip deadSound;

    private GameObject enemyCanvas;

    private GameObject player;

    private PointManager pointManager;

    private float currentEnemyHp;

    private bool soundFlag;

    void Start()
    {
        currentEnemyHp = firstEnemyHp;

        if (enemyHpSlider != null)
        {
            enemyHpSlider.value = 1.0f;
        }

        player = GameObject.Find("Player");

        enemyCanvas = transform.parent.gameObject;

        pointManager = GameObject.Find("Point").GetComponent<PointManager>();
    }

    void Update()
    {
        enemyCanvas.transform.LookAt(player.transform);

        if (currentEnemyHp <= 0)
        {
            WasKilled();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            AttackedByBullet();
        }
    }

    private void AttackedByBullet()
    {
        DOTween.To(() => currentEnemyHp, (x) => currentEnemyHp = x, currentEnemyHp - (everySecondReduceTheAmount / 10f), 0.1f);

        if (enemyHpSlider != null)
        {
            enemyHpSlider.value = currentEnemyHp / firstEnemyHp;
        }

        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);

        GameObject effect = Instantiate(effectPrefab, effectPos, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        effect.transform.forward = -transform.up;

        Destroy(effect, 1.0f);
    }

    public void WasKilled()
    {
        StartCoroutine(FallingBack(true));

        if (!soundFlag)
        {
            AudioSource.PlayClipAtPoint(deadSound, transform.position);

            pointManager.UpdatePoint();

            soundFlag = true;
        }
    }

    private IEnumerator FallingBack(bool waskilled)
    {
        animator.SetBool("Dead", waskilled);

        yield return new WaitForSeconds(1.2f);

        animator.gameObject.SetActive(false);
    }
}