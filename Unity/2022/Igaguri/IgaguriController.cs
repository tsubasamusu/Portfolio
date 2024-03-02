using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgaguriController : MonoBehaviour
{
    float length;

    public float score = 0;

    float px;

    float delta = 0;

    public float span = 1.5f;

    bool stopFlag;

    GameObject target;

    GameObject point;

    GameObject igaguriGenerator;

    PointController pointController;

    IgaguriGenerator generator;

    Vector3 center;

    private void Start()
    {
        this.target = GameObject.Find("target");

        this.point = GameObject.Find("Point");

        this.igaguriGenerator = GameObject.Find("IgaguriGenerator");

        this.generator = this.igaguriGenerator.GetComponent<IgaguriGenerator>();
    }

    void Update()
    {
        Vector3 targetPos = this.target.transform.position;

        this.px = targetPos.x;

        this.center = new Vector3(px, 6.45f, 10f);

        this.delta += Time.deltaTime;

        if (this.delta > this.span)
        {
            Destroy(gameObject);

            this.delta = 0;
        }
    }

    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    public void OnCollisionEnter(Collision other)
    {
        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<ParticleSystem>().Play();

        this.pointController = this.point.GetComponent<PointController>();

        Vector3 dir = this.center - transform.position;

        this.length = dir.magnitude;

        if (this.length <= 2.5f)
        {
            float score2 = 3.5f - this.length;

            this.score = score2 * 10;
        }
        else
        {
            this.score = 0;
        }

        if (this.stopFlag == false)
        {
            pointController.Flag = true;

            pointController.AddScore(this.score);

            this.stopFlag = true;
        }
    }
}