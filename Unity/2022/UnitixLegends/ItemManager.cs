using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;

        [SerializeField]
        private ItemDataSO itemDataSO;

        public ItemDataSO ItemDataSO
        {
            get => itemDataSO;
        }

        private UIManager uIManager;

        private BulletManager bulletManager;

        [SerializeField]
        private Transform itemTrans;

        [SerializeField]
        private Transform generateItemPosPrefab;

        private Transform playerTran;

        [SerializeField]
        private float itemRotSpeed;

        [SerializeField]
        private int maxItemTranCount;

        public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();

        public List<Transform> generatedItemTranList = new List<Transform>();

        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>(5);

        private bool isFull;

        public bool IsFull
        {
            get => isFull;
        }

        private int nearItemNo;

        public int NearItemNo
        {
            get => nearItemNo;
        }

        private float lengthToNearItem;

        public float LengthToNearItem
        {
            get => lengthToNearItem;
        }

        private int selectedItemNo = 0;

        public int SelectedItemNo
        {
            get
            {
                return selectedItemNo;
            }
            set
            {
                selectedItemNo = value;
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetUpItemManager(UIManager uiManager, PlayerController playerController, BulletManager bulletManager)
        {
            this.uIManager = uiManager;

            playerTran = playerController.transform;

            this.bulletManager = bulletManager;

            (List<int> playerItemNums, List<int> enemyItemNums) sortingItemNums = GetSotringItemNums();

            (List<int> playerNums, List<int> enemyNums) GetSotringItemNums()
            {
                List<int> playerList = new();

                List<int> enemyList = new();

                for (int i = 0; i < itemDataSO.itemDataList.Count; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    if (itemDataSO.itemDataList[i].enemyCanUse)
                    {
                        enemyList.Add(itemDataSO.itemDataList[i].itemNo);
                    }
                    else
                    {
                        playerList.Add(itemDataSO.itemDataList[i].itemNo);
                    }
                }

                return (playerList, enemyList);
            }

            GenerateItem(sortingItemNums.playerItemNums, sortingItemNums.enemyItemNums);
        }

        private void Update()
        {
            if (!playerTran)
            {
                return;
            }

            GetInformationOfNearItem(playerTran.position);
        }

        public ItemDataSO.ItemData GetItemData(ItemDataSO.ItemName itemName)
        {
            return itemDataSO.itemDataList.Find(x => x.itemName == itemName);
        }

        public void GenerateItem(List<int> playerItemNums, List<int> enemyItemNums)
        {
            GenerateItemTran();

            CreateGeneratedItemTranList();

            for (int i = 0; i < itemTrans.childCount; i++)
            {
                int px = Random.Range(0, 3);

                if (px != 0)
                {
                    int py = enemyItemNums[Random.Range(0, enemyItemNums.Count)];

                    StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[py].itemPrefab, generatedItemTranList[i])));

                    generatedItemDataList.Add(itemDataSO.itemDataList[py]);

                    continue;
                }

                int pz = playerItemNums[Random.Range(0, playerItemNums.Count)];

                StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[pz].itemPrefab, generatedItemTranList[i])));

                generatedItemDataList.Add(itemDataSO.itemDataList[pz]);
            }
        }

        private void GenerateItemTran()
        {
            for (int i = 0; i < maxItemTranCount; i++)
            {
                Transform generateItemPosTran = Instantiate(generateItemPosPrefab);

                generateItemPosTran.SetParent(itemTrans);

                int px = Random.Range(0, 4);

                float py = Random.Range(-120f, 120f);

                generateItemPosTran.localPosition = px switch
                {
                    0 => new(py, 0f, -120f),
                    1 => new(120f, 0f, py),
                    2 => new(py, 0f, 120f),
                    3 => new(-120f, 0f, py),
                    _ => Vector3.zero,
                };
            }
        }

        private void CreateGeneratedItemTranList()
        {
            for (int i = 0; i < itemTrans.childCount; i++)
            {
                generatedItemTranList.Add(itemTrans.GetChild(i).transform);
            }
        }

        public void GetInformationOfNearItem(Vector3 myPos)
        {
            if (generatedItemTranList.Count <= 0)
            {
                return;
            }

            int itemNo = 0;

            Vector3 nearPos = generatedItemTranList[0].position;

            for (int i = 0; i < generatedItemTranList.Count; i++)
            {
                if (generatedItemTranList[i] == null)
                {
                    continue;
                }

                Vector3 pos = generatedItemTranList[i].position;

                if (Vector3.Scale((pos - myPos), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude)
                {
                    itemNo = i;

                    nearPos = pos;
                }
            }

            nearItemNo = itemNo;

            lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
        }

        private IEnumerator PlayItemAnimation(ItemDetail item)
        {
            if (item != null)
            {
                item.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(item.gameObject);
            }

            while (item != null)
            {
                item.transform.Rotate(0, itemRotSpeed, 0);

                yield return null;
            }
        }

        public void GetItem(int nearItemNo, bool isPlayer, PlayerHealth playerHealth = null)
        {
            if (!isPlayer)
            {
                RemoveItemList(nearItemNo);

                return;
            }

            if (generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                for (int i = 0; i < playerItemList.Count; i++)
                {
                    if (CheckTheElement(i))
                    {
                        playerItemList[i] = generatedItemDataList[nearItemNo];

                        isFull = false;

                        break;
                    }
                }
            }
            else
            {
                CheckIsFull();
            }

            if (isFull && generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                return;
            }

            if (generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Missile || generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Bullet)
            {
                bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }
            else if (generatedItemDataList[nearItemNo].restorativeValue > 0)
            {
                playerHealth.UpdateRecoveryItemCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }

            SetIAlltemSlotSprite();

            StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

            RemoveItemList(nearItemNo);
        }

        public bool CheckTheElement(int elementNo)
        {
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        private void RemoveItemList(int itemNo)
        {
            generatedItemDataList.RemoveAt(itemNo);

            generatedItemTranList.RemoveAt(itemNo);

            Destroy(itemTrans.GetChild(itemNo).gameObject);
        }

        public ItemDataSO.ItemData GetSelectedItemData()
        {
            return playerItemList[SelectedItemNo];
        }

        public void DiscardItem(int itemNo)
        {
            CheckIsFull();

            playerItemList.RemoveAt(itemNo);

            playerItemList.Add(itemDataSO.itemDataList[0]);

            SetIAlltemSlotSprite();
        }

        public void SetIAlltemSlotSprite()
        {
            for (int i = 0; i < playerItemList.Count; i++)
            {
                if (playerItemList[i].itemName == ItemDataSO.ItemName.None)
                {
                    uIManager.imgItemSlotList[i].DOFade(0f, 0f);

                    return;
                }

                uIManager.SetItemSprite(i + 1, playerItemList[i].sprite);
            }
        }

        public void CheckIsFull()
        {
            isFull = true;

            for (int i = 0; i < playerItemList.Count; i++)
            {
                if (CheckTheElement(i))
                {
                    isFull = false;
                }
            }
        }

        public void UseItem(ItemDataSO.ItemData itemData, PlayerHealth playerHealth)
        {
            if (itemData.itemType == ItemDataSO.ItemType.Missile)
            {
                bulletManager.ShotBullet(itemData);
            }
            else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                SoundManager.instance.PlaySE(SeName.RecoverySE);

                playerHealth.UpdatePlayerHp(itemData.restorativeValue);

                playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);

                if (playerHealth.GetRecoveryItemCount(GetSelectedItemData().itemName) == 0)
                {
                    DiscardItem(SelectedItemNo);
                }
            }
            else if (itemData.itemType == ItemDataSO.ItemType.HandWeapon && Input.GetKeyDown(KeyCode.Mouse0))
            {
                bulletManager.PrepareUseHandWeapon(itemData);
            }
        }
    }
}