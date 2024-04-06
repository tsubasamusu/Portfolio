using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public AudioClip appleSE;

    public AudioClip bombSE;

    public AudioClip lightSE;

    public AudioClip starSE;

    public AudioClip clockSE;

    public AudioClip badSE;

    public GameDirector gameDirector;

    AudioSource aud;

    GameObject director;

    GameObject stone;

    public float stopSpan = 3.0f;

    public float stoneSpan = 3.0f;

    float delta = 0;

    float delta2 = 0;

    int px = 0;

    int pz = -1;

    bool stopFlag;

    public bool starFlag;

    public bool clockFlag;

    private void Start()
    {
        this.director = GameObject.Find("GameDirector");

        this.stone = GameObject.Find("Stone");

        this.aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Apple")
        {
            this.director.GetComponent<GameDirector>().GetApple();

            this.aud.PlayOneShot(this.appleSE);
        }
        else if (other.gameObject.tag == "Bomb")
        {
            this.director.GetComponent<GameDirector>().GetBomb();

            this.aud.PlayOneShot(this.bombSE);
        }
        else if (other.gameObject.tag == "LightBall")
        {
            this.stopFlag = true;

            this.aud.PlayOneShot(this.lightSE);
        }
        else if (other.gameObject.tag == "Star")
        {
            this.aud.PlayOneShot(this.starSE);

            this.starFlag = true;
        }
        else if (other.gameObject.tag == "Clock")
        {
            this.aud.PlayOneShot(this.clockSE);

            this.clockFlag = true;
        }

        Destroy(other.gameObject);
    }

    void Update()
    {
        if (gameDirector.time > 0)
        {
            this.delta2 += Time.deltaTime;

            if (this.delta2 > this.stoneSpan)
            {
                this.delta2 = 0;

                this.px = Random.Range(-1, 2);

                this.pz = Random.Range(-1, 2);
            }
        }

        if (transform.position.x != this.px && transform.position.z != this.pz)
        {
            this.stone.transform.position = new Vector3(this.px, 0.5f, this.pz);
        }

        if (this.stopFlag == true)
        {
            this.delta += Time.deltaTime;

            if (this.delta > this.stopSpan)
            {
                this.stopFlag = false;

                this.delta = 0;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                return;
            }

            float x = Mathf.RoundToInt(hit.point.x);

            float z = Mathf.RoundToInt(hit.point.z);

            if (this.px == x && this.pz == z)
            {
                this.aud.PlayOneShot(this.badSE);
            }
            else
            {
                transform.position = new Vector3(x, 0, z);
            }
        }
    }
}