using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField]
    private Texture buttonTexture_Normal;

    [SerializeField]
    private Texture buttonTexture_Active;

    [SerializeField]
    private RawImage rawImage;

    [SerializeField]
    private bool isHomeButton;

    [SerializeField, Header("このボタン以外のボタンのリスト")]
    private List<ButtonColorChanger> otherButtons = new();

    private bool isActive;

    private void Start()
    {
        if (isHomeButton) return;

        rawImage.texture = buttonTexture_Normal;

        isActive = false;
    }

    public void SetNormalTexture()
    {
        isActive = false;

        rawImage.texture = buttonTexture_Normal;
    }

    public void OnClicked()
    {
        foreach (ButtonColorChanger button in otherButtons)
        {
            button.SetNormalTexture();
        }

        if (isHomeButton) return;

        if (isActive) return;

        isActive = !isActive;

        rawImage.texture = buttonTexture_Active;
    }
}