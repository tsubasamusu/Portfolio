using Roulette;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ImageToSetFromCloudStorage
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TextureType textureType;

    public Image Image
    {
        get => image;
    }

    public TextureType TextureType
    {
        get => textureType;
    }
}