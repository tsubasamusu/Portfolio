using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace Roulette
{
    public class UiManagerBase : MonoBehaviour
    {
        [SerializeField]
        private List<SerializableInterface<ISetup>> isetups = new();

        [SerializeField]
        private List<ImageToSetFromCloudStorage> imagesToSetFromCloudStorage = new();

        private void Start()
        {
            OnLoadedClass();

            if (imagesToSetFromCloudStorage != null && imagesToSetFromCloudStorage.Count > 0)
            {
                foreach (ImageToSetFromCloudStorage imageToSetFromCloudStorage in imagesToSetFromCloudStorage)
                {
                    imageToSetFromCloudStorage.Image.sprite = CloudStorageData.Instance.GetSpriteFromTextureType(imageToSetFromCloudStorage.TextureType);
                }
            }

            foreach (SerializableInterface<ISetup> isetup in isetups) isetup.Value.Setup();
        }

        protected virtual void OnLoadedClass()
        {

        }
    }
}