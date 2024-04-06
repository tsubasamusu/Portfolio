using CallOfUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Create WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public enum WeaponName
    {
        AssaultRifle,
        Pistol,
        RocketLauncher,
        Shotgun,
        SniperRifle,
        SubMachineGun
    }

    [Serializable]
    public class WeaponData
    {
        public WeaponName name;

        public GameObject objWeapon;

        public GameObject impactEffect;

        public Sprite sprWeapon;

        public BulletDetailBase bullet;

        public int ammunitionNo;

        public float reloadTime;

        public float rateOfFire;

        public float firingRange;

        public float shotPower;

        [Range(0f, 100f)]
        public float attackPower;

        public AudioClip shotSE;
    }

    public List<WeaponData> weaponDataList = new();
}