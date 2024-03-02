using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace CallOfUnity
{
    public class WeaponButtonDetail : MonoBehaviour
    {
        private WeaponDataSO.WeaponData weaponData;

        public WeaponDataSO.WeaponData WeaponData { get => weaponData; }

        public void SetUpWeaponButton(WeaponDataSO.WeaponData weaponData, UIManager uIManager)
        {
            this.weaponData = weaponData;

            Button btnWeapon = GetComponent<Button>();

            transform.GetChild(0).GetComponent<Text>().text = weaponData.name.ToString();

            btnWeapon.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ïÅí ÇÃÉ{É^ÉìÇâüÇµÇΩéûÇÃâπ);

                    GetComponent<Button>().interactable = false;

                    if (GameData.instance.playerWeaponInfo.info0.data == null)
                    {
                        GameData.instance.playerWeaponInfo.info0.data = weaponData;
                    }
                    else if (GameData.instance.playerWeaponInfo.info1.data == null)
                    {
                        GameData.instance.playerWeaponInfo.info1.data = weaponData;
                    }

                    if (GameData.instance.playerWeaponInfo.info0.data != null && GameData.instance.playerWeaponInfo.info1.data != null)
                    {
                        uIManager.EndChooseWeapon();
                    }
                })
                .AddTo(this);
        }
    }
}