using System;
using UnityEngine;

namespace LightingDemonstration
{
    public struct DownloadedHouseData : IDisposable
    {
        public (GameObject asset, AssetBundle assetBundle) housePrefab;

        public (Cubemap asset, AssetBundle assetBundle) hdri;

        public (TextAsset asset, AssetBundle assetBundle) lightmapData;

        public (Texture2D[] assets, AssetBundle assetBundle) colorLightmaps;

        public (Texture2D[] assets, AssetBundle assetBundle) dirLightmaps;

        public void Dispose()
        {
            //Prefab
            {
                housePrefab.assetBundle.Unload(true);

                housePrefab = (null, null);
            }

            //HDRI
            {
                hdri.assetBundle.Unload(true);

                hdri = (null, null);
            }

            //Lightmap Data
            {
                lightmapData.assetBundle.Unload(true);

                lightmapData = (null, null);
            }

            //Color Lightmap
            {
                colorLightmaps.assetBundle.Unload(true);

                colorLightmaps = (null, null);
            }

            //Dir Lightmap
            {
                dirLightmaps.assetBundle.Unload(true);

                dirLightmaps = (null, null);
            }
        }

        public static bool IsValid((UnityEngine.Object asset, AssetBundle assetBundle) downloadedData)
        {
            return downloadedData.asset != null && downloadedData.assetBundle != null;
        }

        public static bool IsValid((UnityEngine.Object[] assets, AssetBundle assetBundle) downloadedData)
        {
            return downloadedData.assets != null && downloadedData.assetBundle != null;
        }
    }
}