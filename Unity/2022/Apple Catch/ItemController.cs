using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float dropSpeed = -0.03f;

    public GameDirector gameDirector;

    GameObject director;

    private void Start()
    {
        this.director = GameObject.Find("GameDirector");

        this.gameDirector = this.director.GetComponent<GameDirector>();
    }

    void Update()
    {
        if (gameDirector.time <= 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(0, this.dropSpeed, 0);

        if (transform.position.y < -1.0f)
        {
            Destroy(gameObject);
        }
    }
}