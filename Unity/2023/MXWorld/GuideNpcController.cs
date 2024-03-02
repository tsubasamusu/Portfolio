using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Guide
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GuideNpcController : UdonSharpBehaviour
    {
        [SerializeField]
        private Text txtMessage;

        [SerializeField]
        private Transform messageTran;

        private float displayMessageLength;

        private bool setUpedNpc;

        public void SetUpNpc(string message, float displayMessageLength)
        {
            txtMessage.text = message;

            this.displayMessageLength = displayMessageLength;

            messageTran.gameObject.SetActive(false);

            setUpedNpc = true;
        }

        public void Update()
        {
            if (!setUpedNpc) return;

            if (!LocalPlayerIsNearby())
            {
                if (messageTran.gameObject.activeSelf) messageTran.gameObject.SetActive(false);

                return;
            }

            if (!messageTran.gameObject.activeSelf) messageTran.gameObject.SetActive(true);

            messageTran.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
        }

        private bool LocalPlayerIsNearby() => (Networking.LocalPlayer.GetPosition() - transform.position).magnitude <= displayMessageLength;
    }
}