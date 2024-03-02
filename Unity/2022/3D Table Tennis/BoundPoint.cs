using UnityEngine;

public class BoundPoint : MonoBehaviour
{
    [SerializeField, Header("�N�̃R�[�g��")]
    private OwnerType ownerType;

    [SerializeField, Header("���z�ʒu")]
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