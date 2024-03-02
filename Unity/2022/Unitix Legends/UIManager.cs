using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace yamap
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Image eventHorizon;

        [SerializeField]
        private Image logo;

        [SerializeField]
        private Sprite gameStart;

        [SerializeField]
        private Sprite gameClear;

        [SerializeField]
        private Text txtGameOver;

        [SerializeField]
        private Text txtItemCount;

        [SerializeField]
        private Text txtOtherCount;

        [SerializeField]
        private Text txtFps;

        [SerializeField]
        private Text txtMessage;

        [SerializeField]
        private Text floatingMessagePrefab;

        [SerializeField]
        private Slider hpSlider;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Transform itemSlot;

        private BulletManager bulletManager;

        private PlayerHealth playerHealth;

        [SerializeField]
        private Transform floatingMessagesTran;

        [SerializeField]
        private Transform enemies;

        [SerializeField]
        private GameObject itemSlotSetPrefab;

        [SerializeField]
        private GameObject scope;

        [HideInInspector]
        public List<Image> imgItemSlotList = new List<Image>();

        [HideInInspector]
        public List<Image> imgItemSlotBackgroundList = new List<Image>();

        public void SetUpUIManager(BulletManager bulletManager, PlayerHealth playerHealth)
        {
            this.bulletManager = bulletManager;

            this.playerHealth = playerHealth;
        }

        public IEnumerator UpdateText()
        {
            while (true)
            {
                UpdateFpsText();

                UpdateTxtBulletCount();

                yield return null;
            }
        }

        public void SetUpItemSlots()
        {
            GenerateItemSlots(5);

            SetItemSlotBackgroundColor(1, Color.red);
        }

        public void UpdateTxtOtherCount(int enemyNumber)
        {
            txtOtherCount.text = (enemyNumber + 1).ToString() + "Players\n" + GameData.instance.KillCount.ToString() + "Kills";
        }

        public IEnumerator PlayGameStart()
        {
            SetEventHorizonColor(Color.white);

            eventHorizon.DOFade(1f, 0f);

            logo.sprite = gameStart;

            logo.DOFade(1.0f, 2.0f);

            yield return new WaitForSeconds(2.0f);

            eventHorizon.DOFade(0.0f, 1.0f);

            logo.DOFade(0.0f, 1.0f);

            yield return new WaitForSeconds(1.0f);
        }

        public IEnumerator PlayGameClear()
        {
            SetEventHorizonColor(Color.white);

            eventHorizon.DOFade(1f, 0f);

            logo.sprite = gameClear;

            logo.DOFade(1.0f, 2.0f);

            yield return new WaitForSeconds(2.0f);

            logo.DOFade(0.0f, 1.0f);

            yield return new WaitForSeconds(1.0f);
        }

        public IEnumerator PlayGameOver()
        {
            SetEventHorizonColor(Color.black);

            eventHorizon.DOFade(0f, 0f).SetLink(gameObject);

            eventHorizon.DOFade(1.0f, 1.0f).SetLink(gameObject);

            yield return new WaitForSeconds(1.0f);

            txtGameOver.DOText("GameOver", 3.0f).SetEase(Ease.Linear).SetLink(gameObject);

            yield return new WaitForSeconds(4.0f);
        }

        public IEnumerator SetEventHorizonBlack(float time)
        {
            SetEventHorizonColor(Color.black);

            eventHorizon.DOFade(1.0f, 1.0f);

            yield return new WaitForSeconds(time);

            eventHorizon.DOFade(0.0f, 0.5f);
        }

        public void SetEventHorizonColor(Color color)
        {
            eventHorizon.color = color;
        }

        public IEnumerator AttackEventHorizon()
        {
            SetEventHorizonColor(Color.red);

            eventHorizon.DOFade(0.5f, 0.25f);

            yield return new WaitForSeconds(0.25f);

            eventHorizon.DOFade(0.0f, 0.25f);
        }

        public void UpdateHpSliderValue(float currentValue, float updateValue)
        {
            hpSlider.DOValue((currentValue + updateValue) / 100f, 0.5f);
        }

        public void SetCanvasGroup(bool isSetting)
        {
            canvasGroup.alpha = isSetting ? 1.0f : 0.0f;
        }

        private void UpdateTxtBulletCount()
        {
            if (ItemManager.instance.playerItemList.Count == 0)
            {
                return;
            }

            if (ItemManager.instance.GetSelectedItemData().itemType == ItemDataSO.ItemType.Missile)
            {
                txtItemCount.text = bulletManager.GetBulletCount(ItemManager.instance.GetSelectedItemData().itemName).ToString();
            }
            else if (ItemManager.instance.GetSelectedItemData().restorativeValue > 0)
            {
                txtItemCount.text = playerHealth.GetRecoveryItemCount(ItemManager.instance.GetSelectedItemData().itemName).ToString();
            }
            else
            {
                txtItemCount.text = "";
            }
        }

        public void SetFloatingMessagesNotActive()
        {
            floatingMessagesTran.gameObject.SetActive(false);
        }

        public void PrepareGenerateFloatingMessage(string messageText, Color color)
        {
            StartCoroutine(GenerateFloatingMessage(messageText, color));
        }

        public IEnumerator GenerateFloatingMessage(string messageText, Color color)
        {
            Text txtFloatingMessage = Instantiate(floatingMessagePrefab);

            txtFloatingMessage.gameObject.transform.SetParent(floatingMessagesTran);

            txtFloatingMessage.text = messageText;

            txtFloatingMessage.color = color;

            txtFloatingMessage.gameObject.transform.localPosition = Vector3.zero;

            txtFloatingMessage.gameObject.transform.localScale = new Vector3(3f, 3f, 3f);

            Destroy(txtFloatingMessage.gameObject, 3.0f);

            txtFloatingMessage.gameObject.transform.DOLocalMoveY(100.0f, 2.0f);

            yield return new WaitForSeconds(2.0f);

            txtFloatingMessage.DOFade(0.0f, 1.0f);
        }

        private void UpdateFpsText()
        {
            txtFps.text = (1f / Time.deltaTime).ToString("F0");
        }

        private void GenerateItemSlots(int generateNumber)
        {
            for (int i = 0; i < generateNumber; i++)
            {
                GameObject itemSlot = Instantiate(itemSlotSetPrefab, this.itemSlot);

                itemSlot.transform.localScale = Vector3.one;

                itemSlot.transform.localPosition = new Vector3((-1 * (200 + (50 * (generateNumber - 5)))) + (100 * i), -100, 0);

                if (itemSlot.transform.GetChild(2).TryGetComponent<Image>(out Image imgItem))
                {
                    imgItemSlotList.Add(imgItem);

                    imgItem.DOFade(0.0f, 0f);
                }

                if (itemSlot.transform.GetChild(0).TryGetComponent<Image>(out Image imgBackGround))
                {
                    imgItemSlotBackgroundList.Add(imgBackGround);

                    imgBackGround.DOFade(0.3f, 0f);
                }
            }

            itemSlot.localScale = new Vector3(2f, 2f, 2f);

            itemSlot.localPosition = new Vector3(0f, 270f, 0f);
        }

        public void SetItemSprite(int itemNo, Sprite itemSprite)
        {
            imgItemSlotList[itemNo - 1].sprite = itemSprite;

            imgItemSlotList[itemNo - 1].DOFade(1.0f, 0.25f);
        }

        public void SetMessageText(string text, Color color)
        {
            txtMessage.text = text;

            txtMessage.color = color;
        }

        public void SetMessageActive(bool isSetting)
        {
            float value = isSetting ? 1f : 0f;

            txtMessage.DOFade(value, 0f).SetLink(gameObject);
        }

        public void SetItemSlotBackgroundColor(int itemslotNo, Color color)
        {
            for (int i = 0; i < imgItemSlotBackgroundList.Count; i++)
            {
                imgItemSlotBackgroundList[i].color = i == (itemslotNo - 1) ? color : Color.black;

                imgItemSlotBackgroundList[i].DOFade(0.3f, 0f);
            }
        }

        public void PeekIntoTheScope()
        {
            SetCanvasGroup(false);

            scope.gameObject.SetActive(true);
        }

        public void NotPeekIntoTheScope()
        {
            SetCanvasGroup(true);

            scope.gameObject.SetActive(false);
        }

        public void DisplayGameOver()
        {
            SetCanvasGroup(false);

            SetFloatingMessagesNotActive();

            SetMessageActive(false);
        }
    }
}