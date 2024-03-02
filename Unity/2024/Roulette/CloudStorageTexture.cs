using System;
using UnityEngine;

namespace Roulette
{
    [Serializable]
    public class CloudStorageTexture
    {
        [SerializeField]
        private TextureType textureType;

        [SerializeField]
        private string textureName;

        [HideInInspector]
        public Sprite sprite;

        public string TextureName
        {
            get => textureName;
        }

        public TextureType TextureType
        {
            get => textureType;
        }
    }
}