using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yamap
{
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO_yamap")]
    public class ItemDataSO : ScriptableObject
    {
        public enum ItemName
        {
            None,
            Grenade,
            TearGasGrenade,
            Knife,
            Bat,
            Assault,
            Shotgun,
            Sniper,
            Bandage,
            MedicinalPlants,
            Syringe,
            AssaultBullet,
            ShotgunBullet,
            SniperBullet
        }

        public enum ItemType
        {
            Missile,
            HandWeapon,
            Bullet,
            Recovery
        }

        [Serializable]
        public class ItemData
        {
            public ItemName itemName;

            [Range(0.0f, 100.0f)]
            public float restorativeValue;

            [Range(0.0f, 100.0f)]
            public float attackPower;

            public float shotSpeed;

            public float interval;

            public float timeToExplode;

            public int bulletCount;

            public int maxBulletCount;

            public bool enemyCanUse;

            public Sprite sprite;

            [Header("�����Ă�����̃v���t�@�u")]
            public ItemDetail itemPrefab;

            public int itemNo;

            public ItemType itemType;

            [Header("�U�����ɐ�������v���t�@�u")]
            public WeaponBase weaponPrefab;

            public SeName seName;

            public GameObject effectPrefab;
        }

        public List<ItemData> itemDataList = new List<ItemData>();
    }
}