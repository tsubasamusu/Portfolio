using UnityEngine;

namespace LightingDemonstration
{
    public class InformationButton : ButtonBase
    {
        protected override void OnClickedButton() => Application.OpenURL(GameData.Instance.reservedHouseData.houseInformationURL);
    }
}