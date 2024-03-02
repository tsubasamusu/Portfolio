using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyController : MonoBehaviour
{
    public enum PartsName
    {
        �E�r,
        ���r,
        �E��,
        �E�G,
        ����,
        ���G,
    }

    [Serializable]
    public class PartsData
    {
        public PartsName partsName;

        public Transform partsTran;
    }

    [SerializeField]
    private List<PartsData> partsDataList = new List<PartsData>();

    [SerializeField, Header("��莞�ԁi��0.02�b�j���Ƃɉ�]�����鑫�̊p�x"), Range(0f, 10f)]
    private float rotAngle;

    [SerializeField, Range(0f, 80f)]
    private float maxLegAngle;

    [SerializeField, Range(-80f, 0f)]
    private float minLegAngle;

    [SerializeField, Range(0f, 180f)]
    private float maxArmAngle;

    [SerializeField, Range(0f, 10f)]
    private float gravity;

    [SerializeField]
    private Rigidbody rb;

    private List<Vector3> firstPartsLocalEulerAnglesList = new List<Vector3>();

    private int rightLegCount;

    private int leftLegCount;

    public IEnumerator StartControlBody()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                PlayQ();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                PlayW();
            }

            if (Input.GetKey(KeyCode.O))
            {
                PlayO();
            }
            else if (Input.GetKey(KeyCode.P))
            {
                PlayP();
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void SetUpBodyController()
    {
        for (int i = 0; i < partsDataList.Count; i++)
        {
            firstPartsLocalEulerAnglesList.Add(partsDataList[i].partsTran.localEulerAngles);
        }

        Physics.gravity = new Vector3(0f, -gravity, 0f);
    }

    private Transform GetPartsTran(PartsName partsName)
    {
        return partsDataList.Find(x => x.partsName == partsName).partsTran;
    }

    private void PlayQ()
    {
        if (GetLeftLegAngle() >= minLegAngle)
        {
            leftLegCount--;

            UpdateRightArmAngle();

            UpdateLeftKneeAngle();

            GetPartsTran(PartsName.����).Rotate(rotAngle, 0f, 0f);
        }
    }

    private void PlayW()
    {
        if (GetLeftLegAngle() <= maxLegAngle)
        {
            leftLegCount++;

            UpdateRightArmAngle();

            UpdateLeftKneeAngle();

            GetPartsTran(PartsName.����).Rotate(-rotAngle, 0f, 0f);
        }
    }

    private void PlayO()
    {
        if (GetRightLegAngle() >= minLegAngle)
        {
            rightLegCount--;

            UpdateLeftArmAngle();

            UpdateRightKneeAngle();

            GetPartsTran(PartsName.�E��).Rotate(rotAngle, 0f, 0f);
        }
    }

    private void PlayP()
    {
        if (GetRightLegAngle() <= maxLegAngle)
        {
            rightLegCount++;

            UpdateLeftArmAngle();

            UpdateRightKneeAngle();

            GetPartsTran(PartsName.�E��).Rotate(-rotAngle, 0f, 0f);
        }
    }

    private float GetRightLegAngle()
    {
        return rightLegCount * rotAngle;
    }

    private float GetLeftLegAngle()
    {
        return leftLegCount * rotAngle;
    }

    private void UpdateRightArmAngle()
    {
        float currentLegAngleValue = GetLeftLegAngle() < 0f ? Math.Abs(minLegAngle - GetLeftLegAngle()) : GetLeftLegAngle() + Math.Abs(minLegAngle);

        float totalLegAngle = maxLegAngle + Math.Abs(minLegAngle);

        float currentRatio = currentLegAngleValue / totalLegAngle;

        float totalArmAngle = maxArmAngle * 2;

        float firstArmAngle = 180f - maxArmAngle;

        float angleZ = firstArmAngle + (totalArmAngle * currentRatio);

        GetPartsTran(PartsName.�E�r).localEulerAngles = new Vector3(GetPartsTran(PartsName.�E�r).localEulerAngles.x, GetPartsTran(PartsName.�E�r).localEulerAngles.y, angleZ);
    }

    private void UpdateLeftArmAngle()
    {
        float currentLegAngleValue = GetRightLegAngle() < 0f ? Math.Abs(minLegAngle - GetRightLegAngle()) : GetRightLegAngle() + Math.Abs(minLegAngle);

        float totalLegAngle = maxLegAngle + Math.Abs(minLegAngle);

        float currentRatio = currentLegAngleValue / totalLegAngle;

        float totalArmAngle = maxArmAngle * 2;

        float firstArmAngle = 180f - maxArmAngle;

        float angleZ = firstArmAngle + (totalArmAngle * currentRatio);

        GetPartsTran(PartsName.���r).localEulerAngles = new Vector3(GetPartsTran(PartsName.���r).localEulerAngles.x, GetPartsTran(PartsName.���r).localEulerAngles.y, angleZ);
    }

    private void UpdateRightKneeAngle()
    {
        float currentLegAngleValue = GetRightLegAngle() < 0f ? Math.Abs(minLegAngle - GetRightLegAngle()) : GetRightLegAngle() + Math.Abs(minLegAngle);

        float currentRatio = GetRightLegAngle() < 0f ? (Math.Abs(minLegAngle) - currentLegAngleValue) / Math.Abs(minLegAngle) : (currentLegAngleValue - Math.Abs(minLegAngle)) / maxLegAngle;

        float angleX = 89f * currentRatio;

        GetPartsTran(PartsName.�E�G).localEulerAngles = new Vector3(angleX, GetPartsTran(PartsName.�E�G).localEulerAngles.y, GetPartsTran(PartsName.�E�G).localEulerAngles.z);
    }

    private void UpdateLeftKneeAngle()
    {
        float currentLegAngleValue = GetLeftLegAngle() < 0f ? Math.Abs(minLegAngle - GetLeftLegAngle()) : GetLeftLegAngle() + Math.Abs(minLegAngle);

        float currentRatio = GetLeftLegAngle() < 0f ? (Math.Abs(minLegAngle) - currentLegAngleValue) / Math.Abs(minLegAngle) : (currentLegAngleValue - Math.Abs(minLegAngle)) / maxLegAngle;

        float angleX = 89f * currentRatio;

        GetPartsTran(PartsName.���G).localEulerAngles = new Vector3(angleX, GetPartsTran(PartsName.���G).localEulerAngles.y, GetPartsTran(PartsName.���G).localEulerAngles.z);
    }

    public void ResetCharacterCondition(UIManager uIManager)
    {
        rb.isKinematic = true;

        for (int i = 0; i < partsDataList.Count; i++)
        {
            partsDataList[i].partsTran.localEulerAngles = firstPartsLocalEulerAnglesList[i];
        }

        transform.position = transform.eulerAngles = Vector3.zero;

        rightLegCount = leftLegCount = 0;

        uIManager.ResetTimer();

        rb.isKinematic = false;
    }
}