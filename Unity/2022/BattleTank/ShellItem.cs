using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellItem : MonoBehaviour
{
    [SerializeField]
    private AudioClip getSound;

    [SerializeField]
    private GameObject effectPrefab;

    private ShotShell ss;

    [SerializeField]
    private int reward;

    [SerializeField]
    private GameObject shotShell;

    void Start()
    {
        this.ss = this.shotShell.GetComponent<ShotShell>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ss = GameObject.Find("ShotShell").GetComponent<ShotShell>();

            this.ss.AddShell(reward);

            Destroy(gameObject);

            AudioSource.PlayClipAtPoint(getSound, transform.position);

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            Destroy(effect, 0.5f);
        }
    }
}