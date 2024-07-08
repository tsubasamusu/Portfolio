using System;
using UnityEngine;

namespace LightingDemonstration
{
    [Serializable]
    public struct HouseData
    {
        public string houseName;

        public string houseID;

        public string houseInformationURL;

        public string cloudStorageBucketName;

        public string prefabAssetBundleName;

        public string prefabAssetName;

        public string lightmapDataAssetBundleName;

        public string lightmapDataAssetName;

        public string hdriAssetBundleName;

        public string hdriAssetName;

        public string colorLightmapsAssetBundleName;

        public string dirLightmapsAssetBundleName;

        public Sprite thumbnailSprite;

        public Vector3 playerFirstPosition;

        public Vector3 playerFirstAngles;

        public Color skyColor;

        [HideInInspector]
        public DownloadedHouseData downloadedHouseData;

        public bool IsValid() => !string.IsNullOrEmpty(houseName);
    }
}