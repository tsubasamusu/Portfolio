using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsubasa
{
    public class CameraController : MonoBehaviour
    {
        public List<Transform> targetTransList = new();

        [SerializeField]
        private float smooth;

        private void FixedUpdate()
        {
            if (targetTransList.Count == 0)
            {
                return;
            }

            Vector3 pos = new Vector3(GetCenterPos().x, GetCenterPos().y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * smooth);
        }

        private Vector3 GetCenterPos()
        {
            Vector3 totalPos = Vector3.zero;

            for (int i = 0; i < targetTransList.Count; i++)
            {
                totalPos += targetTransList[i].position;
            }

            return totalPos / targetTransList.Count;
        }
    }
}