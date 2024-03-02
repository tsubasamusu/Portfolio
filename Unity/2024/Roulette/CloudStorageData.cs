using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TSUBASAMUSU.CloudStorage;
using UnityEngine;

namespace Roulette
{
    public class CloudStorageData : MonoBehaviour
    {
        [SerializeField]
        private List<CloudStorageTexture> cloudStorageTextures;

        public static CloudStorageData Instance
        {
            get;

            private set;
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

        public async UniTask SetAllSpritesFromCloudStorageAsync()
        {
            for (int i = 0; i < cloudStorageTextures.Count; i++)
            {
                if (cloudStorageTextures[i].sprite != null) continue;

                string googleCloudJwt = await GameData.Instance.GetGoogleCloudJwtAsync();

                if (string.IsNullOrEmpty(googleCloudJwt)) return;

                Texture2D texture2D = await CloudStorageManager.GetTextureFromCloudStorageAsync(googleCloudJwt, SecretConstData.CLOUD_STORAGE_BUCKET_NAME, cloudStorageTextures[i].TextureName);

                if (texture2D == null)
                {
                    ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_CLOUD_STORAGE_OBJECTS);

                    return;
                }

                cloudStorageTextures[i].sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
            }
        }

        public Sprite GetSpriteFromTextureType(TextureType textureType) => cloudStorageTextures.Find(cloudStorageTexture => cloudStorageTexture.TextureType == textureType).sprite;
    }
}