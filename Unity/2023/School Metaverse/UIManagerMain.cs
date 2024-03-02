using DG.Tweening;
using Photon.Pun;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SchoolMetaverse
{
    public class UIManagerMain : MonoBehaviour, ISetUp
    {
        [SerializeField]
        private Image imgMainBackground;

        [SerializeField]
        private Image imgSubBackground;

        [SerializeField]
        private Image imgBlackBord;

        [SerializeField]
        private Image imgNotice;

        [SerializeField]
        private Image imgMute;

        [SerializeField]
        private Button btnSendPicture;

        [SerializeField]
        private Button btnMessage;

        [SerializeField]
        private Button btnMute;

        [SerializeField]
        private Button btnSetting;

        [SerializeField]
        private Button btnSendMessage;

        [SerializeField]
        private Button btnPicturePath;

        [SerializeField]
        private Slider sldBgmVolume;

        [SerializeField]
        private Slider sldLookSensitivity;

        [SerializeField]
        private Slider sldPictureSize;

        [SerializeField]
        private Text txtMessage;

        [SerializeField]
        private Text txtSendPictureError;

        [SerializeField]
        private Text txtMute;

        [SerializeField]
        private InputField ifMessage;

        [SerializeField]
        private InputField ifPicturePath;

        [SerializeField]
        private CanvasGroup cgButton;

        [SerializeField]
        private CanvasGroup cgSetting;

        [SerializeField]
        private CanvasGroup cgMessage;

        [SerializeField]
        private CanvasGroup cgSendPicture;

        [SerializeField]
        private MessageManager messageManager;

        [SerializeField]
        private PictureManager pictureManager;

        [SerializeField]
        private RectTransform rtBlackBord;

        private float firstPictureSize;

        private bool isSettingPictureSize;

        public InputField IfMessage
        {
            get => ifMessage;
        }

        public void SetUp()
        {
            SetUpUI();

            StartControlBtnMessage();

            StartControlBtnSetting();

            StartControlBtnSendPicture();

            StartControlBtnMute();

            StartControlBtnSendMessage();

            StartControlBtnPicturePath();

            StartControlImgNotice();
        }

        private void SetUpUI()
        {
            firstPictureSize = ConstData.PICTURE_SIZE_RATIO * sldPictureSize.value;

            imgMainBackground.color = Color.black;

            cgButton.alpha = 1f;

            imgNotice.gameObject.SetActive(false);

            imgMute.gameObject.SetActive(false);

            txtMute.text = "ミュートする";

            sldBgmVolume.interactable = sldLookSensitivity.interactable = false;

            btnSendMessage.interactable = btnPicturePath.interactable = false;

            cgSetting.alpha = cgMessage.alpha = 0f;

            txtMessage.text = string.Empty;

            imgSubBackground.gameObject.SetActive(false);

            imgBlackBord.gameObject.SetActive(false);

            imgMainBackground.DOFade(0f, ConstData.BACKGROUND_FADE_OUT_TIME)
                .OnComplete(() => Destroy(imgMainBackground.gameObject));
        }

        private void StartControlBtnMessage()
        {
            btnMessage.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
                .Subscribe(_ =>
                {
                    if (cgSetting.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSetting);

                        return;
                    }

                    if (cgSendPicture.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSendPicture);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    if (cgMessage.alpha == 0f)
                    {
                        cgMessage.alpha = 1f;

                        imgSubBackground.gameObject.SetActive(true);

                        ifMessage.interactable = btnSendMessage.interactable = true;

                        ifMessage.text = string.Empty;

                        imgNotice.gameObject.SetActive(false);

                        return;
                    }

                    cgMessage.alpha = 0f;

                    imgSubBackground.gameObject.SetActive(false);

                    ifMessage.interactable = btnSendMessage.interactable = false;
                })
                .AddTo(this);
        }

        private void StartControlBtnSetting()
        {
            btnSetting.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
                .Subscribe(_ =>
                {
                    if (cgMessage.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnMessage);

                        return;
                    }

                    if (cgSendPicture.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSendPicture);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    if (cgSetting.alpha == 0f)
                    {
                        cgSetting.alpha = 1f;

                        imgSubBackground.gameObject.SetActive(true);

                        sldBgmVolume.interactable = sldLookSensitivity.interactable = true;

                        sldLookSensitivity.value = GameData.instance.lookSensitivity / 10f;

                        sldBgmVolume.value = GameData.instance.bgmVolume;

                        return;
                    }

                    cgSetting.alpha = 0f;

                    imgSubBackground.gameObject.SetActive(false);

                    sldBgmVolume.interactable = sldLookSensitivity.interactable = false;

                    GameData.instance.lookSensitivity = sldLookSensitivity.value * 10f;

                    GameData.instance.bgmVolume = sldBgmVolume.value;

                    GameData.instance.SavelookSensitivityInDevice();

                    GameData.instance.SaveBgmVolumeInDevice();

                    SoundManager.instance.UpdateBgmVolume();
                })
                .AddTo(this);
        }

        private void StartControlBtnSendPicture()
        {
            btnSendPicture.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
                .Subscribe(_ =>
                {
                    if (cgMessage.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnMessage);

                        return;
                    }

                    if (cgSetting.alpha != 0f)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        PlayButtonAnimation(btnSetting);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    if (cgSendPicture.alpha == 1f)
                    {
                        cgSendPicture.alpha = 0f;

                        imgSubBackground.gameObject.SetActive(false);

                        ifPicturePath.interactable = btnPicturePath.interactable = false;

                        if (imgBlackBord.sprite == null) return;

                        SetSldPictureSizeActive(false);

                        sldPictureSize.interactable = false;

                        var hashtable = new ExitGames.Client.Photon.Hashtable
                        {
                            ["IsSettingPictureSize"] = false
                        };

                        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

                        isSettingPictureSize = false;

                        return;
                    }

                    cgSendPicture.alpha = 1f;

                    imgSubBackground.gameObject.SetActive(true);

                    ifPicturePath.interactable = btnPicturePath.interactable = true;

                    txtSendPictureError.text = string.Empty;

                    if (imgBlackBord.sprite == null) return;

                    if (!CheckIsSettingPictureSizeOther())
                    {
                        SetSldPictureSizeActive(true);

                        sldPictureSize.interactable = true;

                        if (PhotonNetwork.CurrentRoom.CustomProperties["SldPictureSizeValue"] is float value) sldPictureSize.value = value;

                        var hashtable = new ExitGames.Client.Photon.Hashtable
                        {
                            ["IsSettingPictureSize"] = true
                        };

                        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

                        isSettingPictureSize = true;

                        StartCoroutine(StartUpdatePictureSize());

                        ifPicturePath.text = string.Empty;

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.エラーを表示する時の音);

                    SetTxtSendPictureError("他のプレイヤーが画像のサイズを変更中です。");
                })
                .AddTo(this);
        }

        private void StartControlBtnMute()
        {
            btnMute.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    imgMute.gameObject.SetActive(!imgMute.gameObject.activeSelf);

                    SoundManager.instance.SetRecorderActive(imgMute.gameObject.activeSelf);

                    txtMute.text = imgMute.gameObject.activeSelf ? "ミュートを解除する" : "ミュートする";
                })
                .AddTo(this);
        }

        private void StartControlBtnSendMessage()
        {
            btnSendMessage.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (ifMessage.text == string.Empty)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.送信ボタンを押した時の音);

                    messageManager.PrepareSendMessage(GameData.instance.playerName, ifMessage.text);
                })
                .AddTo(this);
        }

        private void StartControlBtnPicturePath()
        {
            btnPicturePath.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (ifPicturePath.text == string.Empty)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.送信ボタンを押した時の音);

                    pictureManager.SendPicture(ifPicturePath.text);

                    ifPicturePath.text = string.Empty;

                    if (txtSendPictureError.text != string.Empty) return;

                    cgSendPicture.alpha = 0f;

                    imgSubBackground.gameObject.SetActive(false);

                    ifPicturePath.interactable = btnPicturePath.interactable = false;

                    SetSldPictureSizeActive(false);

                    sldPictureSize.interactable = false;

                })
                .AddTo(this);
        }

        private void PlayButtonAnimation(Button button)
        {
            button.transform.DOScale(ConstData.BUTTON_ANIMATION_SIZE, 0.25f).SetLoops(2, LoopType.Yoyo).SetLink(button.gameObject);
        }

        private bool CheckIsSettingPictureSizeOther()
        {
            for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
            {
                if (PhotonNetwork.PlayerListOthers[i].CustomProperties["IsSettingPictureSize"] is bool isSettingPictureSize && isSettingPictureSize) return true;
            }

            return false;
        }

        private IEnumerator StartUpdatePictureSize()
        {
            while (isSettingPictureSize)
            {
                float size = ConstData.PICTURE_SIZE_RATIO * sldPictureSize.value;

                var hashtable = new ExitGames.Client.Photon.Hashtable
                {
                    ["SldPictureSizeValue"] = sldPictureSize.value,
                    ["PictureSize"] = size
                };

                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

                rtBlackBord.localScale = new(size, size, size);

                yield return null;
            }
        }

        private void StartControlImgNotice()
        {
            int latestCount = 0;

            int newCount = 0;

            this.UpdateAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(ConstData.CHECK_MESSAGES_SPAN))
                .Subscribe(_ =>
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties["SendMessageCount"] is not int) return;

                    if (!IncreasedSendMessageCount()) return;

                    if (cgMessage.alpha == 0f) imgNotice.gameObject.SetActive(true);
                })
                .AddTo(this);

            bool IncreasedSendMessageCount()
            {
                newCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["SendMessageCount"];

                if (newCount > latestCount)
                {
                    latestCount = newCount;

                    return true;
                }

                latestCount = newCount;

                return false;
            }
        }

        public void UpdateTxtMessage()
        {
            string message = string.Empty;

            for (int i = 0; i < GameData.instance.messages.Length; i++)
            {
                message += GameData.instance.messages[i];
            }

            txtMessage.text = message;

            ifMessage.text = string.Empty;
        }

        public void SetImgBlackBordSprite(Sprite sprite, float width, float height)
        {
            imgBlackBord.gameObject.SetActive(true);

            rtBlackBord.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            rtBlackBord.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            imgBlackBord.sprite = sprite;

            float size = PhotonNetwork.CurrentRoom.CustomProperties["PictureSize"] is float x ? x : firstPictureSize;

            rtBlackBord.localScale = new(size, size, size);
        }

        public void SetTxtSendPictureError(string text)
        {
            txtSendPictureError.text = text;
        }

        public void SetSldPictureSizeActive(bool isActive)
        {
            sldPictureSize.gameObject.SetActive(isActive);
        }
    }
}