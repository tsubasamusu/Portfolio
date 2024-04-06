using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace StaffMarker
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class StaffMarkerManager : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject markerPrefab;

        [SerializeField, Header("スタッフのユーザー名")]
        private string[] staffNames = new string[0];

        public void Start()
        {
            if (staffNames.Length == 0) return;

            SetUpStaffMarkerManager();
        }

        private void SetUpStaffMarkerManager()
        {
            VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];

            VRCPlayerApi.GetPlayers(players);

            foreach (VRCPlayerApi player in players)
            {
                if (Array.IndexOf(staffNames, player.displayName) == -1) continue;

                VRCInstantiate(markerPrefab).GetComponent<StaffMarkerController>().SetUpStaffMarker(player);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (staffNames.Length == 0) return;

            if (Array.IndexOf(staffNames, player.displayName) == -1) return;

            VRCInstantiate(markerPrefab).GetComponent<StaffMarkerController>().SetUpStaffMarker(player);
        }
    }
}