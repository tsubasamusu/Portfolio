using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private BoundPoint playerBoundPoint;

    [SerializeField]
    private BoundPoint enemyBoundPoint;

    private OwnerType currentOwner = OwnerType.Enemy;

    private bool inCourt;

    private bool stopMove;

    private bool playedBoundSE;

    public bool InCourt { get => inCourt; }

    public OwnerType CurrentOwner { get => currentOwner; }

    public void SetUpBallController()
    {
        transform.position = new Vector3(0f, 1f, -3f);
    }

    public void ShotBall(Vector3 boundPos)
    {
        playedBoundSE = false;

        SoundManager.instance.PlaySound(SoundDataSO.SoundName.RacketSE);

        float posY = currentOwner == OwnerType.Player ? 0.25f : 0.75f;

        Ray ray = new(new Vector3(transform.position.x, posY, transform.position.z), transform.forward);

        inCourt = false;

        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            PrepareMoveBall(boundPos);

            return;
        }

        if (!hit.transform.TryGetComponent(out BoundPoint boundPoint))
        {
            PrepareMoveBall(boundPos);

            return;
        }

        if (boundPoint.GetOwnerTypeOfCourt() == currentOwner)
        {
            inCourt = true;

            PrepareMoveBall(boundPos);
        }
    }

    private float GetAppropriatePosY(Vector3 boundPos)
    {
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(boundPos, new Vector3(1f, 0f, 1f))).magnitude);

        if (!inCourt && transform.position.y <= 0.8f)
        {
            length *= -1f;
        }

        return -(0.75f / 25f) * (length - 5f) * (length - 5f) + 1.5f;
    }

    private void PrepareMoveBall(Vector3 boundPos)
    {
        StartCoroutine(MoveBall(boundPos));
    }

    private IEnumerator MoveBall(Vector3 boundPos)
    {
        OwnerType ownerType = currentOwner;

        while (ownerType == currentOwner)
        {
            if (stopMove)
            {
                break;
            }

            transform.position += transform.forward * GameData.instance.BallSpeed * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(GetAppropriatePosY(boundPos), 0.25f, 10f), transform.position.z);

            if (playedBoundSE)
            {
                yield return null;

                continue;
            }

            if (!InCourt)
            {
                yield return null;

                continue;
            }

            if (transform.position.y <= 0.8f)
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.BoundSE);

                playedBoundSE = true;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RacketController racketController))
        {
            if (currentOwner == racketController.OwnerType)
            {
                return;
            }

            stopMove = false;

            currentOwner = racketController.OwnerType;

            transform.eulerAngles = new Vector3(0f, racketController.transform.root.transform.eulerAngles.y, 0f);

            Vector3 boundPos = (currentOwner == OwnerType.Player ? playerBoundPoint : enemyBoundPoint)
                .GetVirtualBoundPointPos(transform, racketController.transform.root.transform.eulerAngles.y);

            ShotBall(boundPos);
        }
    }

    public void PrepareRestartGame(OwnerType server, PlayerController playerController)
    {
        StartCoroutine(RestartGame(server, playerController));
    }

    private IEnumerator RestartGame(OwnerType server, PlayerController playerController)
    {
        stopMove = true;

        inCourt = false;

        transform.position = new Vector3(0f, 1f, server == OwnerType.Player ? -3f : 3f);

        currentOwner = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;

        playerController.ResetPlayerPos();

        if (server == OwnerType.Enemy)
        {
            yield return new WaitForSeconds(GameData.instance.EnemyServeTime);
        }

        inCourt = true;
    }
}