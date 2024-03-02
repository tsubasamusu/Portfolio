using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace LaserPointer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class LaserPointerController : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject laserPrefab_Blue;

        [SerializeField]
        private GameObject laserPrefab_Green;

        [SerializeField]
        private GameObject laserPrefab_Red;

        [SerializeField]
        private Transform laserOriginTran;

        [SerializeField, Range(1, 3), Header("使用するレーザーの色の番号（①青 ②緑 ③赤）")]
        private int useLaserColorNumber = 1;

        private GameObject objCurrentLaser;

        [UdonSynced]
        private bool syncIsON;

        public void Start()
        {
            objCurrentLaser = VRCInstantiate(GetLaserObject());

            objCurrentLaser.transform.SetParent(laserOriginTran);

            objCurrentLaser.transform.localPosition = Vector3.zero;

            objCurrentLaser.transform.localEulerAngles = Vector3.zero;

            objCurrentLaser.SetActive(false);

            UpdateOnOff();
        }

        private GameObject GetLaserObject()
        {
            switch (useLaserColorNumber)
            {
                case 1: return laserPrefab_Blue;

                case 2: return laserPrefab_Green;

                case 3: return laserPrefab_Red;

                default: return null;
            }
        }

        public void OnPickupAndUseDown()
        {
            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncIsON = !syncIsON;

            RequestSerialization();

            UpdateOnOff();
        }

        public override void OnDeserialization()
        {
            UpdateOnOff();
        }

        private void UpdateOnOff() { if (objCurrentLaser != null) objCurrentLaser.SetActive(syncIsON); }
    }
}