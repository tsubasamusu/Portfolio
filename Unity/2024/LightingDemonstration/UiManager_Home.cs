using System.Collections.Generic;
using UnityEngine;

namespace LightingDemonstration
{
    public class UiManager_Home : UiManagerBase
    {
        [SerializeField]
        private PreviewButton previewButton;

        [HideInInspector]
        public List<HouseButton> houseButtons = new();

        private HouseData selectedHouseData;

        protected override void OnInitialized()
        {
            if (GameData.Instance.reservedHouseData.IsValid()) MemoryUtility.DisposeDownloadedHouseData();
        }

        public void OnClickedHouseButton(HouseButton clickedHouseButton)
        {
            selectedHouseData = clickedHouseButton.HouseData;

            foreach (HouseButton houseButton in houseButtons)
            {
                if (houseButton == clickedHouseButton) continue;

                houseButton.ResetColor();
            }

            previewButton.SetEnable(true);
        }

        public void OnClickedPreviewButton()
        {
            if (!selectedHouseData.IsValid())
            {
                Debug.LogError("The \"" + nameof(selectedHouseData) + "\" has not yet been set.");

                return;
            }

            GameData.Instance.reservedHouseData = selectedHouseData;

            LoadSceneWithFadeOut(ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Load));
        }
    }
}