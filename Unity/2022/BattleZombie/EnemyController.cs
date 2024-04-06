using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private GameObject player;

    private NavMeshAgent agent;

    private Animator animator;

    private bool attackFlag;

    private bool idleFlag;

    [SerializeField]
    private AudioClip attackSound;

    [SerializeField]
    private AudioClip idleSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        player = GameObject.Find("Player");

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        ChasePlayer();

        PlayAnimation();

        ControlSound();
    }

    private void ControlSound()
    {
        if (IsDamageRange() && !attackFlag)
        {
            StartCoroutine(PlayAttackSound());

            attackFlag = true;

            idleFlag = false;
        }
        else if (!IsDamageRange() && !idleFlag)
        {
            StartCoroutine(PlayIdleSound());

            idleFlag = true;

            attackFlag = false;
        }
    }

    private IEnumerator PlayAttackSound()
    {
        while (IsDamageRange())
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);

            yield return new WaitForSeconds(2.5f);
        }
    }

    private IEnumerator PlayIdleSound()
    {
        while (!IsDamageRange())
        {
            AudioSource.PlayClipAtPoint(idleSound, transform.position);

            yield return new WaitForSeconds(20f);
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            agent.destination = player.transform.position;
        }
    }

    private void PlayAnimation()
    {
        animator.SetBool("Walk", !IsDamageRange());

        animator.SetBool("Attack", IsDamageRange());
    }

    private bool IsDamageRange()
    {
        return (player.transform.position - transform.position).magnitude <= playerHealth.damageRange ? true : false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }
    }
}