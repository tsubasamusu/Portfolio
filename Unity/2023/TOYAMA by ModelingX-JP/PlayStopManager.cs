using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace SimpleVideoPlayer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayStopManager : UdonSharpBehaviour
    {
        [SerializeField]
        private VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer videoPlayer;

        [SerializeField]
        private VideoPlayerController videoPlayerController;

        [SerializeField]
        private Image imgBtnPlayStop;

        [SerializeField]
        private bool loop = true;

        [SerializeField]
        private Sprite sprPlay;

        [SerializeField]
        private Sprite sprStop;

        [UdonSynced]
        private bool syncIsPausing;

        public override void OnDeserialization()
        {
            PlayStop();
        }

        public void OnBtnPlayStopClicked()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncIsPausing = !syncIsPausing;

            RequestSerialization();

            videoPlayerController.UpdateSyncPlayTime();

            PlayStop();
        }

        private void PlayStop()
        {
            if (syncIsPausing) videoPlayer.Pause();

            else videoPlayer.Play();

            imgBtnPlayStop.sprite = syncIsPausing ? sprPlay : sprStop;
        }

        public void OnPlayedFromUrl()
        {
            imgBtnPlayStop.sprite = sprStop;

            if (!Networking.IsOwner(gameObject)) return;

            syncIsPausing = false;

            RequestSerialization();
        }

        public void OnVideoEnded()
        {
            if (!loop)
            {
                imgBtnPlayStop.sprite = sprPlay;
            }
            else
            {
                videoPlayer.SetTime(0f);

                videoPlayer.Play();
            }

            if (!Networking.IsOwner(gameObject)) return;

            syncIsPausing = !loop;

            RequestSerialization();
        }
    }
}