using DG.Tweening;
using UnityEngine;

public class RacketController : MonoBehaviour
{
    [SerializeField]
    private OwnerType ownerType;

    private Vector3 normalLocalPos;

    private Vector3 normalLocalRot;

    private bool isIdle;

    private SphereCollider sphereCollider;

    public OwnerType OwnerType { get => ownerType; }

    public bool IsIdle { get => isIdle; }

    public void SetUpRacketController()
    {
        normalLocalPos = transform.localPosition;

        normalLocalRot = transform.localEulerAngles;

        isIdle = true;

        if (TryGetComponent(out sphereCollider))
        {
            sphereCollider.enabled = false;
        }
    }

    public void SetNormalCondition()
    {
        sphereCollider.enabled = false;

        transform.DOLocalMove(normalLocalPos, GameData.instance.PrepareRacketTime);

        transform.DOLocalRotate(normalLocalRot, GameData.instance.PrepareRacketTime)
            .OnComplete(() => isIdle = true);
    }

    public void Drive(bool isForehandDrive)
    {
        isIdle = false;

        sphereCollider.enabled = true;

        Vector3 prepareLocalPos = isForehandDrive ? new Vector3(1f, 0f, 0f) : new Vector3(0.8f, 0f, 1f);

        Vector3 prepareLocalRot = isForehandDrive ? new Vector3(30f, 0f, 270f) : new Vector3(330f, 180f, 270f);

        transform.DOLocalMove(prepareLocalPos, GameData.instance.PrepareRacketTime);

        transform.DOLocalRotate(prepareLocalRot, GameData.instance.PrepareRacketTime)
            .OnComplete(() => transform.DOMove(transform.GetChild(0).transform.position, GameData.instance.SwingTime)
            .OnComplete(() => SetNormalCondition()));
    }
}