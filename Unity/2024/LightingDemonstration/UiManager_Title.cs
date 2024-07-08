using UnityEngine;
using UnityEngine.SceneManagement;
using TSUBASAMUSU.Other;

namespace LightingDemonstration
{
    public class UiManager_Title : UiManagerBase
    {
        public void MoveToHomeScene() => LoadSceneWithFadeOut(ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Home));

        protected override void OnInitialized()
        {
            string houseID = GameData.GetHouseIdFromQueryParameters(WebUtility.GetQueryParameters());

            if (string.IsNullOrEmpty(houseID)) return;

            HouseData houseDataToGo = ConstDataSO.Instance.houseDatas.Find(houseData => houseData.houseID == houseID);

            if (!houseDataToGo.IsValid())
            {
                Debug.LogError("House with house ID \"" + houseID + "\" not found.");

                return;
            }

            GameData.Instance.reservedHouseData = houseDataToGo;

            CgRoot.interactable = false;

            SceneManager.LoadScene(ConstDataSO.Instance.GetSceneNameBySceneType(SceneType.Load));
        }
    }
}