using System;
using UdonSharp;
using UnityEngine;

namespace VideoPlayerBugsPeventer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoPlayerBugsPeventer : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject[] objVideoPlayers;

        [SerializeField, Range(6f, 10f)]
        private float lagTime = 6f;

        private int activeVideoPlayersCount;

        private DateTime[] playedVideoTimes = new DateTime[0];

        private DateTime defaultTime;

        public void Start()
        {
            SetUpPlayedVideoTimes();

            SetVideoPlayersNotActive();
        }

        private void SetUpPlayedVideoTimes()
        {
            defaultTime = DateTime.Now;

            playedVideoTimes = new DateTime[objVideoPlayers.Length];

            for (int i = 0; i < playedVideoTimes.Length; i++)
            {
                playedVideoTimes[i] = defaultTime;
            }
        }

        private void SetVideoPlayersNotActive()
        {
            for (int i = 0; i < objVideoPlayers.Length; i++)
            {
                if (objVideoPlayers[i] == null) continue;

                if (i == 0)
                {
                    objVideoPlayers[i].SetActive(true);

                    activeVideoPlayersCount = 1;

                    continue;
                }

                objVideoPlayers[i].SetActive(false);
            }
        }

        public void Update()
        {
            SetVideoPlayersActive();
        }

        private void SetVideoPlayersActive()
        {
            if (objVideoPlayers.Length == 0) return;

            if (activeVideoPlayersCount == objVideoPlayers.Length) return;

            if (!IsOkToPlayVideo()) return;

            if (objVideoPlayers[activeVideoPlayersCount] == null)
            {
                activeVideoPlayersCount++;

                return;
            }

            objVideoPlayers[activeVideoPlayersCount].SetActive(true);

            playedVideoTimes[activeVideoPlayersCount] = DateTime.Now;

            activeVideoPlayersCount++;
        }

        private bool IsOkToPlayVideo()
        {
            for (int i = 0; i < playedVideoTimes.Length; i++)
            {
                if (GetDifferenceSeconds(DateTime.Now, playedVideoTimes[i]) < lagTime) return false;
            }

            return true;
        }

        private float GetDifferenceSeconds(DateTime newTime, DateTime oldTime) => (float)((newTime - oldTime).TotalSeconds);
    }
}