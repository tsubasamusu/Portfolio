using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TSUBASAMUSU.GoogleCloudJwt;
using UnityEngine;

namespace Roulette
{
    public class GameData : MonoBehaviour
    {
        [SerializeField]
        private string[] googleCloudScopes = new string[0];

        [SerializeField]
        private List<string> unavailableTexts = new();

        private SaveData loginedData;

        public Member winningMember;

        private (string jwt, long issuedUnixTimeSeconds) googleCloudJwt;

        public List<string> UnavailableTexts
        {
            get => unavailableTexts;
        }

        public string[] GoogleCloudScopes
        {
            get => googleCloudScopes;
        }

        public static GameData Instance
        {
            get;
            
            private set;
        }

        [HideInInspector]
        public bool saveLoginInformation;

        public bool Logined
        {
            get => LoginedData != null && !string.IsNullOrEmpty(LoginedData.saveDataName);
        }

        public SaveData LoginedData
        {
            get
            {
                if (loginedData != null) return loginedData;

                if (PlayerPrefs.HasKey(ConstData.SAVE_DATA_NAME)) return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(ConstData.SAVE_DATA_NAME));

                return null;
            }

            set => loginedData = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DeleteStoredSaveData()
        {
            if (PlayerPrefs.HasKey(ConstData.SAVE_DATA_NAME)) PlayerPrefs.DeleteKey(ConstData.SAVE_DATA_NAME);
        }

        public void StoreLoginedData()
        {
            DeleteStoredSaveData();

            PlayerPrefs.SetString(ConstData.SAVE_DATA_NAME, JsonUtility.ToJson(LoginedData));
        }

        public async UniTask<string> GetGoogleCloudJwtAsync()
        {
            if (!string.IsNullOrEmpty(googleCloudJwt.jwt) && googleCloudJwt.issuedUnixTimeSeconds - new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() < ConstData.SPAN_ISSUE_GOOGLE_CLOUD_JWT) return googleCloudJwt.jwt;

            googleCloudJwt = await GoogleCloudJwtGetter.GetGoogleCloudJwtAsync(SecretConstData.GOOGLE_CLOUD_PRIVATE_KEY, SecretConstData.GOOGLE_CLOUD_EMAIL_ADDRESS, googleCloudScopes);

            if (googleCloudJwt.jwt == string.Empty) ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_JWT);

            return googleCloudJwt.jwt;
        }
    }
}