using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace SimpleVideoPlayer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class VideoUrlManager : UdonSharpBehaviour
    {
        [SerializeField]
        private VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer videoPlayer;

        [SerializeField]
        private VRC.SDK3.Components.VRCUrlInputField ifURL;

        [SerializeField]
        private PlayStopManager playStopManager;

        [UdonSynced]
        private VRCUrl syncVrcUrl;

        public void Start()
        {
            if (syncVrcUrl == null) return;

            videoPlayer.Stop();

            videoPlayer.PlayURL(syncVrcUrl);
        }

        public void OnUrlEntered()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncVrcUrl = ifURL.GetUrl();

            RequestSerialization();

            PlayFromUrl();
        }

        public override void OnDeserialization()
        {
            PlayFromUrl();
        }

        public void PlayFromUrl()
        {
            videoPlayer.Stop();

            videoPlayer.PlayURL(syncVrcUrl);

            playStopManager.OnPlayedFromUrl();
        }
    }
}