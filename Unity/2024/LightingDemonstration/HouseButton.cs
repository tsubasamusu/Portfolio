using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LightingDemonstration
{
    public class HouseButton : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Image imgThumbnail;

        [SerializeField]
        private Image imgHouseButtonBackground;

        [SerializeField]
        private TextMeshProUGUI tmpHouseName;

        [SerializeField]
        private ButtonBase buttonBase;

        [SerializeField]
        private float defaultHouseButtonSize;

        private float defaultHouseNameFontSize;

        private Color defaultButtonColor;

        public HouseData HouseData
        {
            get;
            private set;
        }

        private UiManager_Home uiManager;

        public void Setup(HouseData houseData, UiManager_Home uiManager)
        {
            HouseData = houseData;

            this.uiManager = uiManager;

            defaultHouseNameFontSize = tmpHouseName.fontSize;

            imgThumbnail.sprite = houseData.thumbnailSprite;

            tmpHouseName.text = houseData.houseName;

            defaultButtonColor = imgHouseButtonBackground.color;

            buttonBase.Setup();

            rectTransform.ObserveEveryValueChanged(_ => rectTransform.sizeDelta)
                .Subscribe(_ => UpdateHouseNameFontSize())
                .AddTo(this);
        }

        public void OnClickedHouseButton()
        {
            imgHouseButtonBackground.color = Color.black;

            tmpHouseName.color = Color.white;

            uiManager.OnClickedHouseButton(this);
        }

        public void ResetColor()
        {
            if (defaultButtonColor == null)
            {
                Debug.LogError("The " + nameof(HouseButton) + " color could not be reset because the default color of the " + nameof(HouseButton) + " has not yet been gotten.");

                return;
            }

            imgHouseButtonBackground.color = defaultButtonColor;

            tmpHouseName.color = Color.black;
        }

        private void UpdateHouseNameFontSize() => tmpHouseName.fontSize = (rectTransform.sizeDelta.x / defaultHouseButtonSize) * defaultHouseNameFontSize;
    }
}