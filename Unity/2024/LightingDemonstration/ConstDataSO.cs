using System.Collections.Generic;
using UnityEngine;

namespace LightingDemonstration
{
    [CreateAssetMenu(fileName = nameof(ConstDataSO), menuName = "Create " + nameof(ConstDataSO))]
    public class ConstDataSO : ScriptableObject
    {
        private static ConstDataSO instance;

        public static ConstDataSO Instance
        {
            get
            {
                if (instance != null) return instance;

                ConstDataSO constDataSO = Resources.Load<ConstDataSO>(nameof(ConstDataSO));

                if (constDataSO == null)
                {
                    Debug.LogError("Failed to get the \"" + nameof(ConstDataSO) + "\".");

                    return null;
                }

                return instance = constDataSO;
            }
        }

        public string skyboxCubemapParameterName;

        public string skyboxBaseColorParameterName;

        public string googleCloudPrivateKey;

        public string googleCloudEmailAddress;

        public string houseIdQueryName;

        public float textHeightPerSizeOne = 1.45f;

        public float minCameraHeight;

        public float maxCameraHeight;

        [Range(0f, 360f)]
        public float loadingRotateAngle;

        [Range(0f, 1f)]
        public float loadingRotateSpan;

        [Range(0f, 1000f)]
        public float joystickEnableRadius;

        [Range(0f, 1000f)]
        public float joystickMoveableRadius;

        [Range(0f, 1f)]
        public float joystickAnimationTime;

        [Range(0f, 1f)]
        public float cameraAnimationTime;

        [Range(0f, 10f)]
        public float playerMoveSpeedPerSecond;

        [Range(0f, 360f)]
        public float playerRotateAnglePerSecond;

        [Range(0f, 1f)]
        public float UI_AnimationTime;

        [Range(0, 3600)]
        public int issueGoogleCloudJwtSpan;

        public Sprite gyroButtonDefaultSprite;

        public Sprite gyroButtonActiveSprite;

        public HouseButton houseButtonPrefab;

        public TextAsset licenseText;

        public List<SceneData> sceneDatas = new();

        public List<HouseData> houseDatas = new();

        public string[] googleCloudScopes = new string[0];

        public string GetSceneNameBySceneType(SceneType sceneType)
        {
            SceneData sceneData = sceneDatas.Find(sceneData => sceneData.sceneType == sceneType);

            if (!sceneData.IsValid())
            {
                Debug.LogError("Failed to get the scene name type of \"" + sceneType.ToString() + "\".");

                return string.Empty;
            }

            return sceneData.sceneName;
        }
    }
}