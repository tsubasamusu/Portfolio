using UnityEngine;

public class BoundPoint : MonoBehaviour
{
    [SerializeField, Header("誰のコートか")]
    private OwnerType ownerType;

    [SerializeField, Header("仮想位置")]
    private Transform virtualBoundPointTran;

    public OwnerType GetOwnerTypeOfCourt()
    {
        return ownerType;
    }

    public Vector3 GetVirtualBoundPointPos(Transform ballTran, float angleY)
    {
        virtualBoundPointTran.SetParent(ballTran);

        virtualBoundPointTran.localEulerAngles = Vector3.zero;

        virtualBoundPointTran.position = transform.position;

        virtualBoundPointTran.localPosition = new Vector3(0f, virtualBoundPointTran.localPosition.y, virtualBoundPointTran.localPosition.z);

        virtualBoundPointTran.SetParent(transform);

        return virtualBoundPointTran.position;
    }
}