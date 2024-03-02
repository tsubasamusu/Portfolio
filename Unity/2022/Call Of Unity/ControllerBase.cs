using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CallOfUnity
{
    public class ControllerBase : MonoBehaviour, ISetUp
    {
        [HideInInspector]
        public int myTeamNo;

        protected WeaponDataSO.WeaponData currentWeaponData;

        protected int currentWeapoonNo;

        protected int bulletCountForNpc;

        protected Transform weaponTran;

        private Transform weaponParentTran;

        private GameObject objWeapon;

        protected bool isReloading;

        protected bool isPlayer;

        public bool IsPlayer { get => isPlayer; }

        public void SetUp()
        {
            weaponTran = transform.GetChild(0).transform.GetChild(0);

            weaponParentTran = transform.GetChild(0);

            SetUpController();

            ReSetUp();

            DisplayObjWeapon();
        }

        protected void DisplayObjWeapon()
        {
            if (objWeapon != null) Destroy(objWeapon);

            objWeapon = Instantiate(currentWeaponData.objWeapon);

            objWeapon.transform.SetParent(weaponTran.transform);

            objWeapon.transform.localPosition = objWeapon.transform.localEulerAngles = Vector3.zero;
        }

        public virtual void ReSetUp()
        {

        }

        protected virtual void SetUpController()
        {

        }

        protected bool CheckGrounded()
        {
            var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            return Physics.Raycast(ray, 0.15f);
        }

        protected async UniTaskVoid ReloadAsync(CancellationToken token)
        {
            isReloading = true;

            AudioSource.PlayClipAtPoint(SoundManager.instance.GetAudioClip(SoundDataSO.SoundName.リロードする時の音), transform.position);

            if (isPlayer) GameData.instance.UiManager.PlayImgReloadGaugeAnimation(currentWeaponData.reloadTime);

            await UniTask.Delay(TimeSpan.FromSeconds(currentWeaponData.reloadTime), cancellationToken: token);

            SetBulletCount(currentWeapoonNo, currentWeaponData.ammunitionNo);

            isReloading = false;
        }

        protected void Shot()
        {
            if (GetBulletcCount() <= 0) return;

            AudioSource.PlayClipAtPoint(currentWeaponData.shotSE, weaponTran.position);

            SetBulletCount(currentWeapoonNo, Math.Clamp(GetWeaponInfo(currentWeapoonNo).bulletCount - 1, 0, currentWeaponData.ammunitionNo));

            if (isPlayer)
            {
                GameData.instance.playerTotalShotCount++;
            }

            BulletDetailBase bullet = Instantiate(currentWeaponData.bullet);

            bullet.SetUpBullet(currentWeaponData, myTeamNo, isPlayer);

            bullet.transform.SetParent(GameData.instance.TemporaryObjectContainerTran);

            bullet.transform.position = weaponTran.position;

            bullet.transform.forward = weaponTran.forward;

            bullet.transform.GetComponent<Rigidbody>()
                .AddForce(weaponTran.forward * currentWeaponData.shotPower, ForceMode.Impulse);

            Transform effectTran = Instantiate(GameData.instance.ObjMuzzleFlashEffect.transform);

            effectTran.position = weaponTran.position;

            effectTran.SetParent(GameData.instance.TemporaryObjectContainerTran);

            Destroy(effectTran.gameObject, 0.2f);
        }

        protected float GetReloadTime()
        {
            return currentWeaponData.reloadTime;
        }

        public int GetBulletcCount()
        {
            return GetWeaponInfo(currentWeapoonNo).bulletCount;
        }

        protected (WeaponDataSO.WeaponData weaponData, int bulletCount) GetWeaponInfo(int weaponNo)
        {
            if (!isPlayer) return (currentWeaponData, bulletCountForNpc);

            return weaponNo switch
            {
                0 => GameData.instance.playerWeaponInfo.info0,
                1 => GameData.instance.playerWeaponInfo.info1,
                _ => (null, -1)
            };
        }

        protected void SetBulletCount(int weaponNo, int bulletCount)
        {
            if (!isPlayer)
            {
                bulletCountForNpc = bulletCount;

                return;
            }

            switch (weaponNo)
            {
                case 0: GameData.instance.playerWeaponInfo.info0.bulletCount = bulletCount; break;

                case 1: GameData.instance.playerWeaponInfo.info1.bulletCount = bulletCount; break;

                default: Debug.Log("適切な武器の番号を指定してください"); break;
            }
        }
    }
}