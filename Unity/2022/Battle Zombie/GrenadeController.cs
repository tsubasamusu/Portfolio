using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private AudioClip effectSound;

    private void Update()
    {
        Invoke("DestroyG", 3.0f);
    }

    void DestroyG()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(effect, 1.0f);

        AudioSource.PlayClipAtPoint(effectSound, transform.position);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            DestroyG();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.WasKilled();
        }

        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.AttackedByGrenade();
        }
    }
}