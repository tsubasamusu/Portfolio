using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace SimpleVideoPlayer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class VideoPlayerController : UdonSharpBehaviour
    {
        [SerializeField]
        private VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer videoPlayer;

        [SerializeField]
        private PlayStopManager playStopManager;

        [SerializeField]
        private Slider sldPlayTime;

        [SerializeField]
        private Text txtPlayTime;

        [SerializeField]
        private string timeTextFormat = "hh\\:mm\\:ss";

        [SerializeField]
        private float skipValue;

        [SerializeField]
        private float backValue;

        [UdonSynced]
        private float syncPlayTime;

        private float videoDuration;

        public void Update()
        {
            UpdatePlayTimeUI();
        }

        private void UpdatePlayTimeUI()
        {
            float playTime = videoPlayer.GetTime();

            sldPlayTime.value = videoDuration == 0f ? 0f : playTime / videoDuration;

            string playTimeText = new TimeSpan(0, 0, (int)playTime).ToString(timeTextFormat);

            string videoDurationText = new TimeSpan(0, 0, (int)videoDuration).ToString(timeTextFormat);

            txtPlayTime.text = playTimeText + " / " + videoDurationText;
        }

        public override void OnDeserialization()
        {
            if (Networking.IsOwner(gameObject)) return;

            SynchronizePlayTime();
        }

        public void OnBtnSkipClicked()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncPlayTime = videoPlayer.GetTime() + skipValue;

            RequestSerialization();

            SynchronizePlayTime();
        }

        public void OnBtnBackClicked()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncPlayTime = videoPlayer.GetTime() - backValue;

            RequestSerialization();

            SynchronizePlayTime();
        }

        private void SynchronizePlayTime()
        {
            videoPlayer.SetTime(syncPlayTime);
        }

        public override void OnVideoStart()
        {
            videoDuration = videoPlayer.GetDuration();
        }

        public void UpdateSyncPlayTime()
        {
            if (!Networking.IsOwner(gameObject)) return;

            syncPlayTime = videoPlayer.GetTime();

            RequestSerialization();
        }

        public override void OnVideoEnd()
        {
            playStopManager.OnVideoEnded();
        }
    }
}