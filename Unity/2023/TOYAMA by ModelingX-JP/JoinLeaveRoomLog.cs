using System;
using System.Globalization;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace JoinLeaveRoomLog
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class JoinLeaveRoomLog : UdonSharpBehaviour
    {
        [SerializeField]
        private Text txtLog;

        [SerializeField]
        private string joinColorCode = "#00FF00";

        [SerializeField]
        private string dayTimeColorCode = "#FF0000";

        [SerializeField]
        private string leaveColorCode = "#00FF00";

        [SerializeField]
        private string dayDisplayFormat = "M/d";

        [SerializeField]
        private string timeDisplayFormat = "hh:mm";

        [SerializeField, Range(1, 37)]
        private int recordLogCount = 37;

        [UdonSynced]
        private long[] syncTicks;

        [UdonSynced]
        private bool[] syncIsJoinLogs;

        [UdonSynced]
        private string[] syncTargetPlayerNames;

        [UdonSynced]
        private string[] syncLogs;

        public void Start()
        {
            if (!Networking.IsOwner(gameObject)) return;

            syncTicks = new long[recordLogCount];

            syncIsJoinLogs = new bool[recordLogCount];

            syncTargetPlayerNames = new string[recordLogCount];

            syncLogs = new string[recordLogCount];

            for (int i = 0; i < recordLogCount; i++)
            {
                syncTicks[i] = 0;

                syncIsJoinLogs[i] = false;

                syncTargetPlayerNames[i] = string.Empty;
            }

            RequestSerialization();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (!Networking.IsOwner(gameObject)) return;

            if (player == null || !player.IsValid()) return;

            AddLog(DateTime.UtcNow.Ticks, true, player.displayName);

            UpdateLog();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (!Networking.IsOwner(gameObject)) return;

            if (player == null || !player.IsValid()) return;

            AddLog(DateTime.UtcNow.Ticks, false, player.displayName);

            UpdateLog();
        }

        public override void OnDeserialization()
        {
            UpdateLog();
        }

        private void AddLog(long time, bool isJoinLog, string targetPlayerName)
        {
            if (syncTicks == null || syncIsJoinLogs == null || syncTargetPlayerNames == null) return;

            for (int i = 0; i < recordLogCount; i++)
            {
                if (!string.IsNullOrEmpty(syncTargetPlayerNames[i])) continue;

                syncTicks[i] = time;

                syncIsJoinLogs[i] = isJoinLog;

                syncTargetPlayerNames[i] = targetPlayerName;

                RequestSerialization();

                return;
            }

            for (int i = 0; i < recordLogCount; i++)
            {
                syncTicks[i] = i < recordLogCount - 1 ? syncTicks[i + 1] : time;

                syncIsJoinLogs[i] = i < recordLogCount - 1 ? syncIsJoinLogs[i + 1] : isJoinLog;

                syncTargetPlayerNames[i] = i < recordLogCount - 1 ? syncTargetPlayerNames[i + 1] : targetPlayerName;
            }

            RequestSerialization();
        }

        private void UpdateLog()
        {
            if (syncTicks == null || syncIsJoinLogs == null || syncTargetPlayerNames == null) return;

            string logText = string.Empty;

            for (int i = 0; i < recordLogCount; i++)
            {
                if (string.IsNullOrEmpty(syncTargetPlayerNames[i]))
                {
                    if (Networking.IsOwner(gameObject)) syncLogs[i] = string.Empty;

                    continue;
                }

                string dayText = DateTime.Now.ToString(dayDisplayFormat);

                string timeText = new DateTime(syncTicks[i]).ToLocalTime().ToString(timeDisplayFormat, CultureInfo.InvariantCulture);

                string dayTimeText = "<color=" + dayTimeColorCode + ">" + dayText + " " + timeText + "</color>";

                string stateText = "<color=" + (syncIsJoinLogs[i] ? joinColorCode : leaveColorCode) + ">" + (syncIsJoinLogs[i] ? "Joined" : "Left") + "</color>";

                if (Networking.IsOwner(gameObject)) syncLogs[i] = dayTimeText + "　" + syncTargetPlayerNames[i] + " " + stateText;

                logText = logText + syncLogs[i] + "\n";
            }

            txtLog.text = logText;

            if (Networking.IsOwner(gameObject)) RequestSerialization();
        }
    }
}