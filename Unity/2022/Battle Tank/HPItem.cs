using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [SerializeField]
    private AudioClip getSound;

    [SerializeField]
    private int reward;

    private TankHealthSub thSub;

    private GameObject tank;

    void Start()
    {
        this.tank = GameObject.Find("Tank");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.thSub = this.tank.GetComponent<TankHealthSub>();

            thSub.AddHP(reward);

            Destroy(gameObject);

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            Destroy(effect, 0.5f);

            AudioSource.PlayClipAtPoint(getSound, transform.position);
        }
    }
}