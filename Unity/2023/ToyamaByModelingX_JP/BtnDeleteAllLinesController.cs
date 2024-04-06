using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pencil
{
    [RequireComponent(typeof(BoxCollider)), UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BtnDeleteAllLinesButtonController : UdonSharpBehaviour
    {
        [SerializeField]
        private PencilController pencilController;

        public override void Interact()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(PrepareDeleteAllLInes));
        }

        public void PrepareDeleteAllLInes()
        {
            pencilController.DeleteAllLines();
        }

        private void Update()
        {
            transform.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
        }
    }
}