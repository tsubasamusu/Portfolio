using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace StaffMarker
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class StaffMarkerController : UdonSharpBehaviour
    {
        [SerializeField, Header("スタッフマーカーの頭からの高さ")]
        private float heightFromHead = 0.3f;

        [SerializeField, Header("1秒あたりのマーカーの回転角度")]
        private float markerRotationPerSecond = 45f;

        private bool setUped;

        private VRCPlayerApi targetPlayer;

        public void SetUpStaffMarker(VRCPlayerApi targetPlayer)
        {
            this.targetPlayer = targetPlayer;

            setUped = true;
        }

        public void Update()
        {
            if (!setUped) return;

            if (targetPlayer == null || !targetPlayer.IsValid())
            {
                Destroy(gameObject);

                return;
            }

            ControlMarker();
        }

        private void ControlMarker()
        {
            transform.Rotate(0f, markerRotationPerSecond * Time.deltaTime, 0f);

            transform.position = targetPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position + new Vector3(0f, heightFromHead, 0f);
        }
    }
}