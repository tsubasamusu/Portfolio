using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsubasa
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform lookTran;

        [SerializeField]
        private float smooth;

        private Vector3 firstPos;

        private bool set;

        public void SetUpCameraController()
        {
            firstPos = transform.position;

            set = true;
        }

        private void FixedUpdate()
        {
            if (!set)
            {
                return;
            }

            Vector3 pos = new Vector3(firstPos.x, firstPos.y, lookTran.position.z);

            transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * smooth);
        }
    }
}