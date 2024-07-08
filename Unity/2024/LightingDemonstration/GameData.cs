using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TSUBASAMUSU.Google.CloudStorage;
using TSUBASAMUSU.Google.JsonWebToken;
using TSUBASAMUSU.Lighting;
using UnityEngine;
using UnityEngine.Events;

namespace LightingDemonstration
{
    public class GameData : MonoBehaviour
    {
        private (string jwt, long issuedUnixTimeSeconds) googleCloudJwt;

        [HideInInspector]
        public HouseData reservedHouseData;

        private static GameData instance;

        public static GameData Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("The \"" + nameof(instance) + "\" of " + nameof(GameData) + " is null.");

                    return null;
                }

                return instance;
            }

            private set => instance = value;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async UniTaskVoid DownloadAssetsForReservedHouseAsync(UnityEvent onLoadedEvent = null)
        {
            if (!reservedHouseData.IsValid())
            {
                Debug.LogError("The \"" + nameof(reservedHouseData) + "\" is not valid.");

                return;
            }

            string jwt = await GetGoogleCloudJwtAsync();

            if (string.IsNullOrEmpty(jwt)) return;

            if (!(await DownloadHousePrefabAsync(jwt))) return;

            if (!(await DownloadColorLightmapsAsync(jwt))) return;

            if (!(await DownloadDirLightmapsAsync(jwt))) return;

            if (!(await DownloadHdriAsync(jwt))) return;

            if (!(await DownloadLightmapDataAsync(jwt))) return;

            onLoadedEvent?.Invoke();
        }

        private async UniTask<bool> DownloadHousePrefabAsync(string jwt)
        {
            reservedHouseData.downloadedHouseData.housePrefab = await CloudStorageObjectGetter.GetAssetFromCloudStorageAsync<GameObject>(jwt, reservedHouseData.cloudStorageBucketName, reservedHouseData.prefabAssetBundleName, reservedHouseData.prefabAssetName);

            if (!DownloadedHouseData.IsValid(reservedHouseData.downloadedHouseData.housePrefab))
            {
                Debug.LogError("Failed to download house prefab named \"" + reservedHouseData.prefabAssetName + "\".");

                return false;
            }

            return true;
        }

        private async UniTask<bool> DownloadColorLightmapsAsync(string jwt)
        {
            reservedHouseData.downloadedHouseData.colorLightmaps = await CloudStorageObjectGetter.GetAllAssetsFromCloudStorageAsync<Texture2D>(jwt, reservedHouseData.cloudStorageBucketName, reservedHouseData.colorLightmapsAssetBundleName);

            if (!DownloadedHouseData.IsValid(reservedHouseData.downloadedHouseData.colorLightmaps))
            {
                Debug.LogError("Failed to download color lightmaps of \"" + reservedHouseData.prefabAssetBundleName + "\".");

                return false;
            }

            for (int i = 0; i < reservedHouseData.downloadedHouseData.colorLightmaps.assets.Length; i++)
            {
                if (reservedHouseData.downloadedHouseData.colorLightmaps.assets[i] == null)
                {
                    Debug.LogError(i.ToString() + " of downloaded color lightmaps for \"" + reservedHouseData.prefabAssetBundleName + "\" is null.");

                    return false;
                }
            }

            List<Texture2D> downloadedColorLightmaps = reservedHouseData.downloadedHouseData.colorLightmaps.assets.ToList();

            LightingUtility.SortLightmaps(ref downloadedColorLightmaps);

            reservedHouseData.downloadedHouseData.colorLightmaps.assets = downloadedColorLightmaps.ToArray();

            return true;
        }

        private async UniTask<bool> DownloadDirLightmapsAsync(string jwt)
        {
            reservedHouseData.downloadedHouseData.dirLightmaps = await CloudStorageObjectGetter.GetAllAssetsFromCloudStorageAsync<Texture2D>(jwt, reservedHouseData.cloudStorageBucketName, reservedHouseData.dirLightmapsAssetBundleName);

            if (!DownloadedHouseData.IsValid(reservedHouseData.downloadedHouseData.dirLightmaps))
            {
                Debug.LogError("Failed to download dir lightmaps of \"" + reservedHouseData.prefabAssetBundleName + "\".");

                return false;
            }

            for (int i = 0; i < reservedHouseData.downloadedHouseData.dirLightmaps.assets.Length; i++)
            {
                if (reservedHouseData.downloadedHouseData.dirLightmaps.assets[i] == null)
                {
                    Debug.LogError(i.ToString() + " of downloaded dir lightmaps for \"" + reservedHouseData.prefabAssetBundleName + "\" is null.");

                    return false;
                }
            }

            List<Texture2D> downloadedDirLightmaps = reservedHouseData.downloadedHouseData.dirLightmaps.assets.ToList();

            LightingUtility.SortLightmaps(ref downloadedDirLightmaps);

            reservedHouseData.downloadedHouseData.dirLightmaps.assets = downloadedDirLightmaps.ToArray();

            return true;
        }

        private async UniTask<bool> DownloadHdriAsync(string jwt)
        {
            reservedHouseData.downloadedHouseData.hdri = await CloudStorageObjectGetter.GetAssetFromCloudStorageAsync<Cubemap>(jwt, reservedHouseData.cloudStorageBucketName, reservedHouseData.hdriAssetBundleName, reservedHouseData.hdriAssetName);

            if (!DownloadedHouseData.IsValid(reservedHouseData.downloadedHouseData.hdri))
            {
                Debug.LogError("Failed to download HDRI named \"" + reservedHouseData.hdriAssetName + "\".");

                return false;
            }

            return true;
        }

        private async UniTask<bool> DownloadLightmapDataAsync(string jwt)
        {
            reservedHouseData.downloadedHouseData.lightmapData = await CloudStorageObjectGetter.GetAssetFromCloudStorageAsync<TextAsset>(jwt, reservedHouseData.cloudStorageBucketName, reservedHouseData.lightmapDataAssetBundleName, reservedHouseData.lightmapDataAssetName);

            if (!DownloadedHouseData.IsValid(reservedHouseData.downloadedHouseData.lightmapData))
            {
                Debug.LogError("Failed to download lightmap data JSON file named \"" + reservedHouseData.lightmapDataAssetName + "\".");

                return false;
            }

            return true;
        }

        public async UniTask<string> GetGoogleCloudJwtAsync()
        {
            if (!string.IsNullOrEmpty(googleCloudJwt.jwt) && googleCloudJwt.issuedUnixTimeSeconds - new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() < ConstDataSO.Instance.issueGoogleCloudJwtSpan) return googleCloudJwt.jwt;

            googleCloudJwt = await GoogleCloudJwtGetter.GetGoogleCloudJwtAsync(ConstDataSO.Instance.googleCloudPrivateKey, ConstDataSO.Instance.googleCloudEmailAddress, ConstDataSO.Instance.googleCloudScopes);

            if (string.IsNullOrEmpty(googleCloudJwt.jwt))
            {
                Debug.LogError("Failed to get JSON Web Token of Google Cloud.");

                return null;
            }

            return googleCloudJwt.jwt;
        }

        public static string GetHouseIdFromQueryParameters(Dictionary<string, string> queryParameters)
        {
            if (queryParameters == null) return string.Empty;

            if (queryParameters.TryGetValue(ConstDataSO.Instance.houseIdQueryName, out string houseId)) return houseId;

            Debug.LogError("Failed to get a house ID from the query parameters.");

            return string.Empty;
        }
    }
}