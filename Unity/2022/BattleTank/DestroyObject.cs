using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private GameObject effectPrefab2;

    public int objectHP;

    [SerializeField]
    private GameObject itemPrefab1;

    [SerializeField]
    private GameObject itemPrefab2;

    [SerializeField]
    private GameObject itemPrefab3;

    private GameObject itemPrefab;

    private AudioSource audioS;

    [SerializeField]
    private AudioClip explosionSE;

    void Start()
    {
        this.audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shell") || other.CompareTag("EnemyShell"))
        {
            objectHP -= 1;

            this.audioS.PlayOneShot(this.explosionSE);

            if (objectHP > 0)
            {
                Destroy(other.gameObject);

                GameObject effect = Instantiate(effectPrefab, other.transform.position, Quaternion.identity);

                Destroy(effect, 2.0f);
            }
            else
            {
                Destroy(other.gameObject);

                GameObject effect2 = Instantiate(effectPrefab2, other.transform.position, Quaternion.identity);

                Destroy(effect2, 2.0f);

                Destroy(this.gameObject);

                int px = Random.Range(0, 3);

                switch (px)
                {
                    case 0: itemPrefab = itemPrefab1; break;

                    case 1: itemPrefab = itemPrefab2; break;

                    case 2: itemPrefab = itemPrefab3; break;
                }

                Vector3 pos = transform.position;

                Instantiate(itemPrefab, new Vector3(pos.x, pos.y + 0.6f, pos.z), Quaternion.identity);
            }
        }
    }
}