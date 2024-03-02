using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace PrivateRoom
{
    [RequireComponent(typeof(SphereCollider)), UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class LockButtonController : UdonSharpBehaviour
    {
        [SerializeField]
        private Transform doorTran_R;

        [SerializeField]
        private Transform doorTran_L;

        [SerializeField, Header("ドアの開閉に要する時間")]
        private float doorAnimationTime = 1f;

        [SerializeField, Header("ドアの最大角度"), Range(0f, 180f)]
        private float maxDoorAngle = 80f;

        [UdonSynced]
        private bool syncIsOpen = true;

        private float rotAnglePerSecond;

        public void Start()
        {
            rotAnglePerSecond = maxDoorAngle / doorAnimationTime;

            doorTran_R.localEulerAngles = new Vector3(0f, maxDoorAngle, 0f);

            doorTran_L.localEulerAngles = new Vector3(0f, -maxDoorAngle, 0f);
        }

        public void Update()
        {
            if (syncIsOpen && doorTran_R.localEulerAngles.y >= maxDoorAngle)
            {
                doorTran_R.localEulerAngles = new Vector3(0f, maxDoorAngle, 0f);

                doorTran_L.localEulerAngles = new Vector3(0f, -maxDoorAngle, 0f);

                return;
            }

            if (!syncIsOpen && ((maxDoorAngle < doorTran_R.localEulerAngles.y && doorTran_R.localEulerAngles.y <= 360f) || doorTran_R.localEulerAngles.y == 0f))
            {
                doorTran_R.localEulerAngles = Vector3.zero;

                doorTran_L.localEulerAngles = Vector3.zero;

                return;
            }

            RotateDoors();
        }

        private void RotateDoors()
        {
            float rotAngle = rotAnglePerSecond * Time.deltaTime;

            doorTran_R.Rotate(0f, syncIsOpen ? rotAngle : -rotAngle, 0f);

            doorTran_L.Rotate(0f, syncIsOpen ? -rotAngle : rotAngle, 0f);
        }

        public override void Interact()
        {
            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncIsOpen = !syncIsOpen;

            RequestSerialization();
        }
    }
}