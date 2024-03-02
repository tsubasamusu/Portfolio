using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEditor.PackageManager.Requests;

namespace CallOfUnity
{
    public class UIManager : MonoBehaviour, ISetUp
    {
        private enum LogoType
        {
            GameTitle,
            GameOver,
            GameClear
        }

        [Serializable]
        private class LogoData
        {
            public LogoType logoType;

            public Sprite sprite;
        }

        [SerializeField]
        private List<LogoData> logoDatasList = new();

        [SerializeField]
        private Image imgLogo;

        [SerializeField]
        private Image imgBackground;

        [SerializeField]
        private CanvasGroup cgGameUI;

        [SerializeField]
        private CanvasGroup cgOtherButtons;

        [SerializeField]
        private Text txtScoreTeam0;

        [SerializeField]
        private Text txtScoreTeam1;

        [SerializeField]
        private Button btnMain;

        [SerializeField]
        private Button btnSetting;

        [SerializeField]
        private Button btnChooseWeapon;

        [SerializeField]
        private Button btnData;

        [SerializeField]
        private Image imgMainButton;

        [SerializeField]
        private Text txtMainButton;

        [SerializeField]
        private CanvasGroup cgSettings;

        [SerializeField]
        private Slider sldLookSensitivity;

        [SerializeField]
        private Slider sldLookSmooth;

        [SerializeField]
        private Text txtData;

        [SerializeField]
        private Slider sldHp;

        [SerializeField]
        private Text txtBulletCount;

        [SerializeField]
        private Image imgReloadGauge;

        [SerializeField]
        private Toggle tglHideMouseCursor;

        [SerializeField]
        private Text txtGaveDamage;

        [SerializeField]
        private WeaponButtonDetail BtnWeaponPrefab;

        [HideInInspector]
        public ReactiveProperty<bool> EndedGameStartPerformance = new(false);

        [HideInInspector]
        public ReactiveProperty<bool> EndedGameEndPerformance = new(false);

        private List<WeaponButtonDetail> btnWeaponList = new();

        private Tween reloadGaugeTween;

        public void SetUp()
        {
            Reset();

            btnMain.OnClickAsObservable()
                .Where(_ => !EndedGameStartPerformance.Value)
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(_ =>
                {
                    if (GameData.instance.playerWeaponInfo.info0.data == null || GameData.instance.playerWeaponInfo.info1.data == null)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnChooseWeapon);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ゲームスタートボタンを押した時の音);

                    btnMain.interactable = cgOtherButtons.interactable = false;

                    imgBackground.DOFade(0f, 1f);

                    imgLogo.DOFade(0f, 1f);

                    cgOtherButtons.DOFade(0f, 1f);

                    txtMainButton.DOFade(0f, 1f).OnComplete(() => cgGameUI.alpha = 1f);

                    imgMainButton.DOFade(0f, 1f).OnComplete(() => EndedGameStartPerformance.Value = true);
                })
                .AddTo(this);

            btnSetting.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(_ =>
                {
                    if (txtData.color.a != 0)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnData);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.普通のボタンを押した時の音);

                    if (cgSettings.gameObject.activeSelf)
                    {
                        GameData.instance.lookSensitivity = sldLookSensitivity.value * 10f;

                        GameData.instance.lookSmooth = sldLookSmooth.value;

                        GameData.instance.hideMouseCursor = tglHideMouseCursor.isOn;

                        cgSettings.gameObject.SetActive(false);

                        btnMain.gameObject.SetActive(true);
                    }
                    else
                    {
                        cgSettings.gameObject.SetActive(true);

                        btnMain.gameObject.SetActive(false);
                    }
                })
                .AddTo(this);

            btnData.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(_ =>
                {
                    if (cgSettings.gameObject.activeSelf)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSetting);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.普通のボタンを押した時の音);

                    if (txtData.color.a == 1f)
                    {
                        txtData.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

                        btnMain.gameObject.SetActive(true);
                    }
                    else
                    {
                        txtData.color = Color.black;

                        btnMain.gameObject.SetActive(false);
                    }
                })
                .AddTo(this);

            RectTransform canvasRectTran = GameObject.Find("Canvas").GetComponent<RectTransform>();

            btnChooseWeapon.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(_ =>
                {
                    if (txtData.color.a != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnData);

                        return;
                    }
                    else if (cgSettings.gameObject.activeSelf)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSetting);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.普通のボタンを押した時の音);

                    btnMain.interactable = btnSetting.interactable = btnChooseWeapon.interactable = btnData.interactable = false;

                    imgLogo.DOFade(0f, 1f);

                    imgMainButton.DOFade(0f, 1f);

                    txtMainButton.DOFade(0f, 1f);

                    cgOtherButtons.DOFade(0f, 1f)
                        .OnComplete(() =>
                        {
                            for (int i = 0; i < GameData.instance.WeaponDataSO.weaponDataList.Count; i++)
                            {
                                WeaponButtonDetail btnWeapon = Instantiate(BtnWeaponPrefab);

                                btnWeapon.SetUpWeaponButton(GameData.instance.WeaponDataSO.weaponDataList[i], this);

                                RectTransform btnWeaponRectTran = btnWeapon.GetComponent<RectTransform>();

                                btnWeaponRectTran.SetParent(canvasRectTran);

                                float y = -400f + (800f / (GameData.instance.WeaponDataSO.weaponDataList.Count - 1) * i);

                                btnWeaponRectTran.localPosition = new Vector3(0f, y, 0f);

                                btnWeaponList.Add(btnWeapon);
                            }
                        });
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ => txtBulletCount.text = GameData.instance.PlayerControllerBase.GetBulletcCount().ToString())
                .AddTo(this);

            void PlayButtonAnimation(Button button)
            {
                button.gameObject.transform.DOScale(1.3f, 0.25f).SetLoops(2, LoopType.Yoyo);
            }
        }

        private void Reset()
        {
            tglHideMouseCursor.isOn = GameData.instance.hideMouseCursor;

            txtGaveDamage.text = string.Empty;

            btnMain.interactable = cgOtherButtons.interactable = false;

            cgGameUI.alpha = cgOtherButtons.alpha = imgReloadGauge.fillAmount = 0f;

            cgSettings.alpha = 1f;

            sldLookSensitivity.value = GameData.instance.lookSensitivity / 10f;

            sldLookSmooth.value = GameData.instance.lookSmooth;

            sldHp.value = 1f;

            UpdateTxtScore();

            cgSettings.gameObject.SetActive(false);

            txtData.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

            imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);

            imgLogo.sprite = GetLogoSprite(LogoType.GameTitle);

            imgMainButton.color = Color.blue;

            txtMainButton.text = "Game Start";

            txtData.text = "Total Kill : " + GameData.instance.playerTotalKillCount.ToString() + "\n"
                + "Kill-Death Ratio : " + (GameData.instance.playerTotalKillCount / (GameData.instance.playerTotalDeathCount == 0 ? 1f : GameData.instance.playerTotalDeathCount)).ToString("F2") + "\n"
                + "Hit Rate : " + (GameData.instance.playerTotalAttackCount / (GameData.instance.playerTotalShotCount == 0 ? 1f : GameData.instance.playerTotalShotCount)).ToString("F2") + "%";

            imgMainButton.DOFade(0f, 0f);

            txtMainButton.DOFade(0f, 0f);

            imgLogo.DOFade(1f, 1f);

            txtMainButton.DOFade(1f, 1f);

            cgOtherButtons.DOFade(1f, 1f).OnComplete(() => cgOtherButtons.interactable = true);

            imgMainButton.DOFade(1f, 1f).OnComplete(() => btnMain.interactable = true);
        }

        private Sprite GetLogoSprite(LogoType logoType)
        {
            return logoDatasList.Find(x => x.logoType == logoType).sprite;
        }

        public void EndChooseWeapon()
        {
            while (btnWeaponList.Count > 0)
            {
                Destroy(btnWeaponList[0].gameObject);

                btnWeaponList.RemoveAt(0);
            }

            Destroy(btnChooseWeapon.gameObject);

            imgLogo.DOFade(1f, 1f);

            imgMainButton.DOFade(1f, 1f);

            txtMainButton.DOFade(1f, 1f);

            cgOtherButtons.DOFade(1f, 1f)
                .OnComplete(() => btnMain.interactable = btnSetting.interactable = btnData.interactable = true);
        }

        public void SetSldHp(float setValue)
        {
            sldHp.DOValue(setValue, 0.25f);
        }

        public void UpdateTxtScore()
        {
            txtScoreTeam0.text = GameData.instance.Score.Value.team0.ToString();

            txtScoreTeam1.text = GameData.instance.Score.Value.team1.ToString();
        }

        public void PlayImgReloadGaugeAnimation(float animationTime)
        {
            imgReloadGauge.fillAmount = 1f;

            reloadGaugeTween = imgReloadGauge.DOFillAmount(0f, animationTime).SetEase(Ease.Linear);
        }

        public void StopReloadGaugeAnimation()
        {
            if (reloadGaugeTween == null) return;

            reloadGaugeTween.Kill();

            imgReloadGauge.fillAmount = 0f;
        }

        public void PlayTxtGaveDamageAnimation(float gaveDamage)
        {
            Sequence sequence = DOTween.Sequence();

            txtGaveDamage.text = gaveDamage.ToString();

            sequence.Append(txtGaveDamage.DOFade(1f, 0f));

            sequence.Append(txtGaveDamage.gameObject.transform.DOScale(1.3f, 0.25f).SetLoops(2, LoopType.Yoyo));

            sequence.Append(txtGaveDamage.DOFade(0f, 0.25f));
        }

        public void PlayGameEndPerformance(bool isGameClear)
        {
            btnMain.interactable = false;

            txtMainButton.text = "Restart";

            imgMainButton.color = isGameClear ? Color.yellow : Color.red;

            imgBackground.color = isGameClear ? Color.white : Color.black;

            imgLogo.sprite = GetLogoSprite(isGameClear ? LogoType.GameClear : LogoType.GameOver);

            cgGameUI.alpha = 0f;

            imgBackground.DOFade(1f, 1f);

            imgLogo.DOFade(1f, 1f);

            imgMainButton.DOFade(1f, 1f);

            txtMainButton.DOFade(1f, 1f).OnComplete(() => btnMain.interactable = true);

            btnMain.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.普通のボタンを押した時の音);

                    btnMain.interactable = false;

                    imgLogo.DOFade(0f, 1f);

                    imgMainButton.DOFade(0f, 1f);

                    txtMainButton.DOFade(0f, 1f);

                    imgBackground.DOColor(Color.white, 1f).OnComplete(() => EndedGameEndPerformance.Value = true);
                })
                .AddTo(this);
        }
    }
}