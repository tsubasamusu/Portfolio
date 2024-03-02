using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace CallOfUnity
{
    public class GameData : MonoBehaviour, ISetUp
    {
        [Range(0f, 10f)]
        public float lookSensitivity;

        [Range(0f, 1f)]
        public float lookSmooth;

        [HideInInspector]
        public int playerTotalKillCount;

        [HideInInspector]
        public int playerTotalDeathCount;

        [HideInInspector]
        public int playerTotalAttackCount;

        [HideInInspector]
        public int playerTotalShotCount;

        [HideInInspector]
        public bool hideMouseCursor;

        [SerializeField]
        private List<Transform> respawnTransList = new();

        [SerializeField]
        private GameObject objMuzzleFlashEffect;

        [SerializeField]
        private GameObject objBleedingEffect;

        [SerializeField]
        private GameObject objRocketLauncherEffect;

        [SerializeField]
        private GameObject objExplosionEffect;

        [SerializeField]
        private GameObject objImpactBulletEffect;

        [SerializeField]
        private Transform temporaryObjectContainerTran;

        [SerializeField]
        private ControllerBase npcControllerBase;

        [SerializeField]
        private ControllerBase playerControllerBase;

        [SerializeField]
        private CharacterHealth playerCharacterHealth;

        [SerializeField]
        private UIManager uiManager;

        [SerializeField]
        private Material team0Material;

        [SerializeField]
        private Material team1Material;

        [HideInInspector]
        public ReactiveProperty<(int team0, int team1)> Score = new((0, 0));

        [SerializeField]
        private WeaponDataSO weaponDataSO;

        [SerializeField]
        private SoundDataSO soundDataSO;

        [HideInInspector]
        public List<ControllerBase> npcControllerBaseList = new();

        [HideInInspector]
        public ((WeaponDataSO.WeaponData data, int bulletCount) info0, (WeaponDataSO.WeaponData data, int bulletCount) info1) playerWeaponInfo;

        public List<Transform> RespawnTransList { get => respawnTransList; }

        public GameObject ObjMuzzleFlashEffect { get => objMuzzleFlashEffect; }

        public GameObject ObjBleedingEffect { get => objBleedingEffect; }

        public GameObject ObjRocketLauncherEffect { get => objRocketLauncherEffect; }

        public GameObject ObjExplosionEffect { get => objExplosionEffect; }

        public GameObject ObjImpactBulletEffect { get => objImpactBulletEffect; }

        public Transform TemporaryObjectContainerTran { get => temporaryObjectContainerTran; }

        public ControllerBase NpcControllerBase { get => npcControllerBase; }

        public ControllerBase PlayerControllerBase { get => playerControllerBase; }

        public CharacterHealth PlayerCharacterHealth { get => playerCharacterHealth; }

        public UIManager UiManager { get => uiManager; }

        public Material Team0Material { get => team0Material; }

        public Material Team1Material { get => team1Material; }

        public WeaponDataSO WeaponDataSO { get => weaponDataSO; }

        public SoundDataSO SoundDataSO { get => soundDataSO; }

        public static GameData instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetUp()
        {
            Reset();
        }

        private void Reset()
        {
            if (PlayerPrefs.HasKey("Kill")) playerTotalKillCount = PlayerPrefs.GetInt("Kill");

            if (PlayerPrefs.HasKey("Death")) playerTotalDeathCount = PlayerPrefs.GetInt("Death");

            if (PlayerPrefs.HasKey("Attack")) playerTotalAttackCount = PlayerPrefs.GetInt("Attack");

            if (PlayerPrefs.HasKey("Shot")) playerTotalShotCount = PlayerPrefs.GetInt("Shot");

            if (PlayerPrefs.HasKey("LookSensitivity")) lookSensitivity = PlayerPrefs.GetFloat("LookSensitivity");

            if (PlayerPrefs.HasKey("LookSmooth")) lookSmooth = PlayerPrefs.GetFloat("LookSmooth");

            if (PlayerPrefs.HasKey("HideMouseCursor")) hideMouseCursor = PlayerPrefs.GetString("HideMouseCursor") == true.ToString();
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("Kill", playerTotalKillCount);

            PlayerPrefs.SetInt("Death", playerTotalDeathCount);

            PlayerPrefs.SetInt("Attack", playerTotalAttackCount);

            PlayerPrefs.SetInt("Shot", playerTotalShotCount);

            PlayerPrefs.SetFloat("LookSensitivity", lookSensitivity);

            PlayerPrefs.SetFloat("LookSmooth", lookSmooth);

            PlayerPrefs.SetString("HideMouseCursor", hideMouseCursor.ToString());
        }
    }
}