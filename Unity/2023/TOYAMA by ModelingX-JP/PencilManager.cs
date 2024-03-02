using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Pencil
{
    [RequireComponent(typeof(VRCPickup), typeof(VRCObjectSync)), UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PencilManager : UdonSharpBehaviour
    {
        [SerializeField]
        private PencilController pencilController;

        [SerializeField]
        private GameObject objButtons;

        public override void OnPickup()
        {
            objButtons.SetActive(false);
        }

        public override void OnDrop()
        {
            LookAtOwner();

            objButtons.SetActive(true);
        }

        public override void OnPickupUseDown()
        {
            pencilController.OnPickupAndUseDown();
        }

        public override void OnPickupUseUp()
        {
            pencilController.OnPickupAndUseUp();
        }

        private void LookAtOwner()
        {
            transform.LookAt(Networking.GetOwner(pencilController.gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
        }
    }
}