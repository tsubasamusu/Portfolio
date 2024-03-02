using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAttackItem : MonoBehaviour
{
    private GameObject[] targets;

    [SerializeField]
    private AudioClip getSound;

    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private float stopAttackSpan;

    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("EnemyShotShell");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].GetComponent<EnemyShotShellSub>().AddStopTimer(this.stopAttackSpan);
            }

            Destroy(gameObject);

            AudioSource.PlayClipAtPoint(getSound, transform.position);

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            Destroy(effect, 0.5f);
        }
    }
}