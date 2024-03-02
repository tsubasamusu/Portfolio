using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject applePrefab;

    public GameObject bombPrefab;

    public GameObject lightBallPrefab;

    public GameObject starPrefab;

    public GameObject clockPrefab;

    public BasketController basketController;

    GameObject item;

    public float span = 0.5f;

    float span2;

    public float bonusSpan = 3.0f;

    float delta = 0;

    float speed = -0.03f;

    void Start()
    {
        this.span2 = this.span;
    }

    void Update()
    {
        if (basketController.starFlag == true)
        {
            this.delta += Time.deltaTime;

            if (this.delta > this.bonusSpan)
            {
                this.delta = 0;

                this.span = this.span2;

                basketController.starFlag = false;
            }
            else if (this.delta <= this.bonusSpan)
            {
                this.span = 0.1f;

                item = Instantiate(applePrefab);
            }
        }
        else
        {
            this.delta += Time.deltaTime;

            if (this.delta > this.span)
            {
                this.delta = 0;

                int dice = Random.Range(1, 6);

                switch (dice)
                {
                    case 1:
                        item = Instantiate(applePrefab);
                        break;

                    case 2:
                        item = Instantiate(clockPrefab);
                        break;

                    case 3:
                        item = Instantiate(bombPrefab);
                        break;

                    case 4:
                        item = Instantiate(lightBallPrefab);
                        break;

                    case 5:
                        item = Instantiate(starPrefab);
                        break;

                }

            }
        }

        float x = Random.Range(-1, 2);

        float z = Random.Range(-1, 2);

        item.transform.position = new Vector3(x, 4, z);

        item.GetComponent<ItemController>().dropSpeed = this.speed;
    }
}