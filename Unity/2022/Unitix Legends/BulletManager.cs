using System.Collections;
using UnityEngine;

namespace yamap
{
    public class BulletManager : MonoBehaviour
    {
        private Transform mainCamera;

        private Transform temporaryObjectContainerTran;

        private PlayerController playerController;

        private float timer;

        private bool stopFlag;

        private int grenadeBulletCount;

        public int GrenadeBulletCount
        {
            get
            {
                return grenadeBulletCount;
            }
        }

        private int tearGasGrenadeBulletCount;

        public int TearGasGrenadeBulletCount
        {
            get
            {
                return tearGasGrenadeBulletCount;
            }
        }

        private int assaultBulletCount;

        public int AssaultBulletCount
        {
            get
            {
                return assaultBulletCount;
            }
        }

        private int sniperBulletCount;

        public int SniperBulletCount
        {
            get
            {
                return sniperBulletCount;
            }
        }

        private int shotgunBulletCount;

        public int ShotgunBulletCount
        {
            get
            {
                return shotgunBulletCount;
            }
        }

        public void SetUpBulletManager(PlayerController playerController, Transform temporaryObjectContainerTran)
        {
            mainCamera = Camera.main.transform;

            this.playerController = playerController;

            this.temporaryObjectContainerTran = temporaryObjectContainerTran;

            StartCoroutine(MeasureTime());
        }

        private void Update()
        {
            transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
        }

        private IEnumerator MeasureTime()
        {
            while (true)
            {
                timer += Time.deltaTime;

                yield return null;
            }
        }

        public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue)
        {
            ref int useBulletCount = ref GetUseBulletCount(itemName);

            int maxBulletCount = GetUseMaxBulletCount(itemName);

            useBulletCount = Mathf.Clamp(useBulletCount + updateValue, 0, maxBulletCount);
        }

        int x = 0;

        private ref int GetUseBulletCount(ItemDataSO.ItemName itemName)
        {
            switch (itemName)
            {
                case ItemDataSO.ItemName.Grenade: return ref grenadeBulletCount;

                case ItemDataSO.ItemName.TearGasGrenade: return ref tearGasGrenadeBulletCount;

                case ItemDataSO.ItemName.Assault: return ref assaultBulletCount;

                case ItemDataSO.ItemName.Shotgun: return ref shotgunBulletCount;

                case ItemDataSO.ItemName.Sniper: return ref sniperBulletCount;

                default: return ref x;
            }
        }

        private int GetUseMaxBulletCount(ItemDataSO.ItemName itemName)
        {
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        public void ShotBullet(ItemDataSO.ItemData itemData)
        {
            if (timer < itemData.interval || GetBulletCount(itemData.itemName) == 0)
            {
                return;
            }

            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Player, transform.forward * itemData.shotSpeed, itemData.seName, itemData.interval, itemData.effectPrefab);

            bullet.gameObject.transform.SetParent(temporaryObjectContainerTran);

            UpdateBulletCount(itemData.itemName, -1);

            timer = 0;

            if (itemData.itemName == ItemDataSO.ItemName.Grenade || itemData.itemName == ItemDataSO.ItemName.TearGasGrenade)
            {
                if (GetBulletCount(itemData.itemName) <= 0)
                {
                    ItemManager.instance.DiscardItem(ItemManager.instance.SelectedItemNo);
                }
            }
        }

        public int GetBulletCount(ItemDataSO.ItemName itemName)
        {
            switch (itemName)
            {
                case ItemDataSO.ItemName.Grenade: return grenadeBulletCount;

                case ItemDataSO.ItemName.TearGasGrenade: return tearGasGrenadeBulletCount;

                case ItemDataSO.ItemName.Assault: return assaultBulletCount;

                case ItemDataSO.ItemName.Sniper: return sniperBulletCount;

                case ItemDataSO.ItemName.Shotgun: return shotgunBulletCount;

                default: return 0;
            }
        }

        public void PrepareUseHandWeapon(ItemDataSO.ItemData itemData)
        {
            StartCoroutine(UseHandWeapon(itemData));
        }

        public IEnumerator UseHandWeapon(ItemDataSO.ItemData itemData)
        {
            if (stopFlag)
            {
                yield break;
            }

            stopFlag = true;

            HandWeaponDetailBase handWeapon = Instantiate(itemData.weaponPrefab, transform).GetComponent<HandWeaponDetailBase>();

            handWeapon.SetUpWeaponDetail(itemData.attackPower, BulletOwnerType.Player, itemData.seName, itemData.interval);

            handWeapon.gameObject.transform.localPosition = Vector3.zero;

            yield return new WaitForSeconds(itemData.interval);

            stopFlag = false;
        }
    }
}