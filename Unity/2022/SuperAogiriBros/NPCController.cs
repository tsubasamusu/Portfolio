using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTran;

    [SerializeField]
    private GameObject attackPoint;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private CharacterManager.CharaName myName;

    private bool isAttack;

    private bool isJumping;

    private float currentMoveSpeed;

    private bool soundFlag;

    private bool jumped;

    private void Start()
    {
        attackPoint.SetActive(false);

        currentMoveSpeed = GameData.instance.npcMoveSpeed;

        StartCoroutine(Move());
    }

    private void Update()
    {
        if (enemyTran == null)
        {
            return;
        }

        if (CheckCliff())
        {
            StartCoroutine(ClingingCliff());

            return;
        }

        if (Mathf.Abs(enemyTran.position.x) > 7f)
        {
            currentMoveSpeed = 0f;

            animator.SetBool("Run", false);

            return;
        }

        if (Mathf.Abs(enemyTran.position.x - transform.position.x) <= 0.5f)
        {
            currentMoveSpeed = 0f;

            if (!isJumping)
            {
                StartCoroutine(Jump());
            }

            return;
        }

        if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f && Mathf.Abs(enemyTran.position.y - transform.position.y) < 2)
        {
            if (!isAttack)
            {
                animator.SetBool("Run", false);

                animator.SetBool("Jump", false);

                currentMoveSpeed = 0f;

                StartCoroutine(Attack());
            }

            return;
        }

        currentMoveSpeed = GameData.instance.npcMoveSpeed;

        if (enemyTran.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        else if (enemyTran.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        if (isAttack)
        {
            return;
        }

        if (CheckGrounded())
        {
            soundFlag = false;

            jumped = false;

            animator.SetBool("Run", true);
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;

        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

        animator.SetBool("Jump", true);

        rb.AddForce(transform.up * GameData.instance.npcJumpPower);

        yield return new WaitForSeconds(1.8f);

        animator.SetBool("Jump", false);

        isJumping = false;
    }

    private bool CheckGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        float tolerance = 0.3f;

        return Physics.Raycast(ray, tolerance);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (enemyTran == null)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                continue;
            }

            if (Mathf.Abs(transform.position.x) > 7f && transform.position.y < 0f)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                continue;
            }

            rb.AddForce(transform.forward * currentMoveSpeed);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private IEnumerator Attack()
    {
        isAttack = true;

        SoundManager.instance.PlaySound(SoundManager.instance.GetCharacterVoiceData(myName).clip);

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.3f);

        attackPoint.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        attackPoint.SetActive(false);

        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
    }

    private bool CheckCliff()
    {
        if (transform.position.y > -1f || transform.position.y < -3f)
        {
            return false;
        }

        if (transform.position.x < -9f || transform.position.x > 9f)
        {
            return false;
        }

        return true;
    }

    private IEnumerator ClingingCliff()
    {
        if (jumped)
        {
            yield break;
        }

        if (!soundFlag)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Cliff).clip);

            soundFlag = true;
        }

        animator.SetBool("Attack", false);

        animator.SetBool("Jump", false);

        animator.SetBool("Run", false);

        animator.SetBool("Cliff", true);

        transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);

        yield return new WaitForSeconds(1f);

        if (!jumped)
        {
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

            transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

            animator.SetBool("Cliff", false);

            animator.SetBool("Jump", true);

            jumped = true;
        }
    }
}