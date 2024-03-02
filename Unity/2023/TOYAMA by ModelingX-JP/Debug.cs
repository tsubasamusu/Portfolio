using System;
using System.Globalization;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Debugger
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Debug : UdonSharpBehaviour
    {
        [SerializeField]
        private Text txtDebug;

        [SerializeField]
        private GameObject emptyObjectPrefab;

        [SerializeField, Range(1, 28), Header("デバッグ用テキストの最大行数")]
        private int maxTextLines = 28;

        [SerializeField, Header("プレイヤー追従時のプレイヤーの頭との距離")]
        private float distanceFromHead = 1f;

        [SerializeField, Header("デバッグ用テキスト表示時にその時の時刻も表示する")]
        private bool displayTime = true;

        [SerializeField, Header("時刻表示形式")]
        private string timeDisplayFormat = "HH:mm:ss.fff";

        private Transform emptyObjectTran;

        private string[] debugTexts = new string[0];

        private int setDebugTextCount;

        public void Update()
        {
            FollowLocalPlayer();
        }

        private void FollowLocalPlayer()
        {
            if (emptyObjectTran == null)
            {
                emptyObjectTran = VRCInstantiate(emptyObjectPrefab).transform;

                transform.SetParent(emptyObjectTran);

                transform.localPosition = new Vector3(0f, 0f, distanceFromHead);
            }

            VRCPlayerApi.TrackingData headData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

            emptyObjectTran.position = headData.position;

            emptyObjectTran.eulerAngles = headData.rotation.eulerAngles;
        }

        public void Log(object message)
        {
            setDebugTextCount++;

            string debugText = message.ToString();

            if (displayTime) debugText = GetAddedTimeText(debugText);

            UpdateDebugTexts(debugText);

            UpdateUiText();
        }

        private string GetAddedTimeText(string originalText)
        {
            DateTime currentTime = DateTime.Now;

            string timeText = currentTime.ToString(timeDisplayFormat, CultureInfo.InvariantCulture) + "　";

            return timeText + originalText;
        }

        private void UpdateDebugTexts(string addValue)
        {
            if (debugTexts.Length != maxTextLines) debugTexts = new string[maxTextLines];

            if (setDebugTextCount <= debugTexts.Length)
            {
                debugTexts[setDebugTextCount - 1] = addValue;

                return;
            }

            string[] debugTextsBefore = new string[debugTexts.Length];

            Array.Copy(debugTexts, debugTextsBefore, debugTexts.Length);

            for (int i = 0; i < debugTexts.Length; i++)
            {
                if (i == debugTexts.Length - 1)
                {
                    debugTexts[i] = addValue;

                    continue;
                }

                debugTexts[i] = debugTextsBefore[i + 1];
            }
        }

        private void UpdateUiText()
        {
            string text = string.Empty;

            for (int i = 0; i < debugTexts.Length; i++)
            {
                text = text + debugTexts[i] + "\n";
            }

            txtDebug.text = text;
        }
    }
}