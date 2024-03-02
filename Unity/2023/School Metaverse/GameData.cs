using System;
using UniRx;
using UnityEngine;

namespace SchoolMetaverse
{
    public class GameData : MonoBehaviour
    {
        [HideInInspector]
        public string playerName;

        [HideInInspector]
        public float lookSensitivity = 5f;

        [HideInInspector]
        public float bgmVolume = 1f;

        [HideInInspector]
        public string[] messages = new string[ConstData.MAX_MESSAGE_LINES];

        public static GameData instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("PlayerName")) playerName = PlayerPrefs.GetString("PlayerName");

            if (PlayerPrefs.HasKey("LookSensitivity")) lookSensitivity = PlayerPrefs.GetFloat("LookSensitivity");

            if (PlayerPrefs.HasKey("BgmVolume")) bgmVolume = PlayerPrefs.GetFloat("BgmVolume");
        }

        public void SavePlayerNameInDevice()
        {
            PlayerPrefs.SetString("PlayerName", playerName);
        }

        public void SavelookSensitivityInDevice()
        {
            PlayerPrefs.SetFloat("LookSensitivity", lookSensitivity);
        }

        public void SaveBgmVolumeInDevice()
        {
            PlayerPrefs.SetFloat("BgmVolume", bgmVolume);
        }
    }
}