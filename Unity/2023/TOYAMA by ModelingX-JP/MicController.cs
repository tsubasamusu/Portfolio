using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mic
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class MicController : UdonSharpBehaviour
    {
        [SerializeField]
        private float voiceDistance_Mic = 1000f;

        [SerializeField]
        private float voiceDistance_Default = 10f;

        [UdonSynced]
        private int syncHavingMicPlayerId = -1;

        public void Start()
        {
            UpdateVoiceDistance();
        }

        public void OnPickupMic()
        {
            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncHavingMicPlayerId = Networking.LocalPlayer.playerId;

            RequestSerialization();

            UpdateVoiceDistance();
        }

        public void OnDropMic()
        {
            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncHavingMicPlayerId = -1;

            RequestSerialization();

            UpdateVoiceDistance();
        }

        public override void OnDeserialization()
        {
            UpdateVoiceDistance();
        }

        private void UpdateVoiceDistance()
        {
            VRCPlayerApi havingMicPlayer = syncHavingMicPlayerId == -1 ? null : VRCPlayerApi.GetPlayerById(syncHavingMicPlayerId);

            VRCPlayerApi[] allPlayers = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];

            VRCPlayerApi.GetPlayers(allPlayers);

            foreach (VRCPlayerApi player in allPlayers)
            {
                if (player == null || !player.IsValid()) continue;

                float voiceDistance = player == havingMicPlayer ? voiceDistance_Mic : voiceDistance_Default;

                SetVoiceDistance(player, voiceDistance);
            }
        }

        private void SetVoiceDistance(VRCPlayerApi targetPlayer, float voiceDistance)
        {
            if (targetPlayer == null || !targetPlayer.IsValid()) return;

            targetPlayer.SetVoiceDistanceNear(voiceDistance);

            targetPlayer.SetVoiceDistanceFar(voiceDistance);

            targetPlayer.SetVoiceVolumetricRadius(voiceDistance);
        }
    }
}