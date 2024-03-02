using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap
{
    public class PlayerHealth : MonoBehaviour
    {
        private UIManager uiManager;

        private float playerHp = 100.0f;

        public float PlayerHp
        {
            get
            {
                return playerHp;
            }
        }

        private int bandageCount;

        public int BandageCount
        {
            get
            {
                return bandageCount;
            }
        }

        private int medicinalPlantscount;

        public int MedicinalPlantsCount
        {
            get
            {
                return medicinalPlantscount;
            }
        }

        private int syringeCount;

        public int SyringeCount
        {
            get
            {
                return syringeCount;
            }
        }

        public void SetUpHealth(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        private void OnCollisionEnter(Collision hit)
        {
            if (hit.gameObject.TryGetComponent(out ItemDetail itemDetail))
            {
                UpdatePlayerHp(itemDetail.GetAttackPower(), hit.gameObject);
            }
        }

        public void UpdatePlayerHp(float updateValue, GameObject gameObject = null)
        {
            if (updateValue < 0.0f)
            {
                StartCoroutine(uiManager.AttackEventHorizon());
            }

            uiManager.UpdateHpSliderValue(playerHp, updateValue);

            playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        private void AttackedByTearGasGrenade()
        {
            StartCoroutine(uiManager.SetEventHorizonBlack(5.0f));
        }

        public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue)
        {
            ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);

            int maxCount = GetRecoveryItemMaxCount(itemName);

            recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);
        }

        int x = 0;

        private ref int GetRecoveryItemCountRef(ItemDataSO.ItemName itemName)
        {
            switch (itemName)
            {
                case ItemDataSO.ItemName.Bandage: return ref bandageCount;

                case ItemDataSO.ItemName.MedicinalPlants: return ref medicinalPlantscount;

                case ItemDataSO.ItemName.Syringe: return ref syringeCount;

                default: return ref x;
            }
        }

        private int GetRecoveryItemMaxCount(ItemDataSO.ItemName itemName)
        {
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        public int GetRecoveryItemCount(ItemDataSO.ItemName itemName)
        {
            return itemName switch
            {
                ItemDataSO.ItemName.Bandage => BandageCount,
                ItemDataSO.ItemName.MedicinalPlants => MedicinalPlantsCount,
                ItemDataSO.ItemName.Syringe => SyringeCount,
                _ => 0,
            };
        }
    }
}