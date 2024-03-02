using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mic
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PopUpController : UdonSharpBehaviour
    {
        [SerializeField]
        private Transform popUpTran;

        [SerializeField]
        private float popUpHeightFromMic = 2f;

        public void Update()
        {
            popUpTran.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);

            popUpTran.position = new Vector3(transform.position.x, transform.position.y + popUpHeightFromMic, transform.position.z);
        }
    }
}