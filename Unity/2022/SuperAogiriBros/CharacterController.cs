using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Tsubasa
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private GameObject attackPoint;

        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private CharacterManager.CharaName myName;

        private float moveDirection;

        private float cliffTimer;

        private bool isjumping;

        private bool isAttack;

        private bool jumped;

        private bool soundFlag;

        public void SetUpCharacterController(CharacterManager characterManager)
        {
            attackPoint.SetActive(false);

            StartCoroutine(Move(characterManager));
        }

        private IEnumerator Move(CharacterManager characterManager)
        {
            yield return new WaitForSeconds(0.5f);

            while (true)
            {
                if (CheckCliff())
                {
                    cliffTimer += Time.fixedDeltaTime;

                    if (cliffTimer < GameData.instance.maxCliffTime)
                    {
                        ClingingCliff(characterManager);
                    }
                    else
                    {
                        animator.SetBool("Cliff", false);
                    }

                    yield return new WaitForSeconds(Time.fixedDeltaTime);

                    continue;
                }

                soundFlag = false;

                cliffTimer = 0f;

                StartCoroutine(ControlMovement(characterManager));

                ControlAnimation();

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        private IEnumerator ControlMovement(CharacterManager characterManager)
        {
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Right)))
            {
                transform.eulerAngles = new Vector3(0f, -90f, 0f);

                moveDirection = 1f;
            }
            else if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Left)))
            {
                transform.eulerAngles = new Vector3(0f, 90f, 0f);

                moveDirection = -1f;
            }
            else
            {
                moveDirection = 0f;
            }

            rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * GameData.instance.moveSpeed);

            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Down)) && !isAttack)
            {
                StartCoroutine(Attack());
            }

            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)) && !isjumping)
            {
                isjumping = true;

                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                rb.AddForce(transform.up * GameData.instance.jumpPower);

                yield return new WaitForSeconds(1.8f);

                isjumping = false;
            }
        }

        private bool CheckGrounded()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            float tolerance = 0.3f;

            return Physics.Raycast(ray, tolerance);
        }

        private void ControlAnimation()
        {
            animator.SetBool("Cliff", false);

            if (isAttack)
            {
                animator.SetBool("Jump", false);

                animator.SetBool("Run", false);

                return;
            }

            if (isjumping)
            {
                animator.SetBool("Run", false);

                animator.SetBool("Jump", true);

                return;
            }
            else
            {
                animator.SetBool("Jump", false);
            }

            if (!CheckGrounded())
            {
                return;
            }

            jumped = false;

            if (moveDirection != 0f)
            {
                animator.SetBool("Run", true);

                return;
            }
            else
            {
                animator.SetBool("Run", false);
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

        private void ClingingCliff(CharacterManager characterManager)
        {
            if (jumped)
            {
                return;
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

            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)))
            {
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                jumped = true;

                return;
            }

            transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

            transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
        }
    }
}