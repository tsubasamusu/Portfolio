using System.Collections.Generic;
using TSUBASAMUSU.Lighting;
using UnityEngine;

namespace LightingDemonstration
{
    public class HouseDataApplyer : MonoBehaviour, ISetup
    {
        public void Setup()
        {
            //Prefab
            {
                GameObject generatedPrefab = Instantiate(GameData.Instance.reservedHouseData.downloadedHouseData.housePrefab.asset);

                generatedPrefab.transform.position = Vector3.zero;

                SetHousePrefabStatic(generatedPrefab);
            }

            //Lightmap
            {
                LightingUtility.ApplyLightmapsToCurrentScene(GameData.Instance.reservedHouseData.downloadedHouseData.colorLightmaps.assets, GameData.Instance.reservedHouseData.downloadedHouseData.dirLightmaps.assets);

                if (!LightingUtility.ApplyLightmapsToMeshRenderers(GameData.Instance.reservedHouseData.downloadedHouseData.lightmapData.asset))
                {
                    Debug.LogError("Failed to apply lightmap data to MeshRenderers in \"" + ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Preview) + "\".");

                    return;
                }
            }

            ApplyDownloadedHDRI();

            DestroyAllLights();
        }

        private void SetHousePrefabStatic(GameObject generatedPrefab)
        {
            generatedPrefab.isStatic = true;

            foreach (GameObject childGameObject in GetAllChildren(generatedPrefab))
            {
                childGameObject.isStatic = true;
            }
        }

        public List<GameObject> GetAllChildren(GameObject parentGameObject)
        {
            List<GameObject> allChildrenGameObjects = new();

            GetChildrenRecursive(parentGameObject, allChildrenGameObjects);

            return allChildrenGameObjects;

            static void GetChildrenRecursive(GameObject parentGameObject, List<GameObject> allChildrenGameObjects)
            {
                foreach (Transform childTransform in parentGameObject.transform)
                {
                    allChildrenGameObjects.Add(childTransform.gameObject);

                    GetChildrenRecursive(childTransform.gameObject, allChildrenGameObjects);
                }
            }
        }

        private void ApplyDownloadedHDRI()
        {
            Material skyboxMaterial = RenderSettings.skybox;

            if (skyboxMaterial == null)
            {
                Debug.LogError("The Skybox of \"" + ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Preview) + "\" is null.");

                return;
            }

            skyboxMaterial.SetTexture(ConstDataSO.Instance.skyboxCubemapParameterName, GameData.Instance.reservedHouseData.downloadedHouseData.hdri.asset);

            skyboxMaterial.SetColor(ConstDataSO.Instance.skyboxBaseColorParameterName, GameData.Instance.reservedHouseData.skyColor);
        }

        private void DestroyAllLights()
        {
            foreach (Light light in FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                Destroy(light.gameObject);
            }
        }
    }
}