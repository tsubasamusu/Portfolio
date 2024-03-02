using Photon.Pun;
using System;
using System.Drawing;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SchoolMetaverse
{
    public class PictureManager : MonoBehaviour, ISetUp
    {
        [SerializeField]
        private UIManagerMain uiManagerMain;

        public void SetUp()
        {
            this.UpdateAsObservable()
                .Where(_ => PhotonNetwork.CurrentRoom.CustomProperties["IsSettingPicture"] is bool isSettingPicture && !isSettingPicture)
                .Where(_ => PhotonNetwork.CurrentRoom.CustomProperties["PictureBites"] is byte[])
                .ThrottleFirst(System.TimeSpan.FromSeconds(ConstData.PICTURE_SYNCHRONIZE_SPAN))
                .Subscribe(_ => SetPictureFromBytes((byte[])PhotonNetwork.CurrentRoom.CustomProperties["PictureBites"]))
                .AddTo(this);
        }

        public void SendPicture(string picturePath)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["IsSettingPicture"] is bool isSettingPicture && isSettingPicture)
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.エラーを表示する時の音);

                uiManagerMain.SetTxtSendPictureError("他のプレイヤーが画像を送信中です。");

                return;
            }

            var hashtable = new ExitGames.Client.Photon.Hashtable
            {
                ["IsSettingPicture"] = true
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

            Image imgPicture;

            try
            {
                imgPicture = Image.FromFile(picturePath);
            }
            catch (FileNotFoundException)
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.エラーを表示する時の音);

                uiManagerMain.SetSldPictureSizeActive(false);

                uiManagerMain.SetTxtSendPictureError("正しい画像のパスを入力してください。\n入力されたパス\n" + picturePath);

                var hashtable1 = new ExitGames.Client.Photon.Hashtable
                {
                    ["IsSettingPicture"] = false
                };

                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable1);

                return;
            }

            if (imgPicture.Width >= ConstData.MAX_PICTURE_SIZE || imgPicture.Height >= ConstData.MAX_PICTURE_SIZE)
            {
                imgPicture = imgPicture.GetThumbnailImage(imgPicture.Width / ConstData.DIVIDE_BIG_PICTURE_VALUE, imgPicture.Height / ConstData.DIVIDE_BIG_PICTURE_VALUE, delegate { return false; }, IntPtr.Zero);
            }

            ImageConverter imageConverter = new();

            byte[] bytes = (byte[])imageConverter.ConvertTo(imgPicture, typeof(byte[]));

            SetPictureFromBytes(bytes);
        }

        private void SetPictureFromBytes(byte[] bytes)
        {
            Texture2D texture = new(1, 1);

            if (!texture.LoadImage(bytes))
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.エラーを表示する時の音);

                uiManagerMain.SetSldPictureSizeActive(false);

                uiManagerMain.SetTxtSendPictureError("画像のテクスチャを作成できませんでした。\n開発者に問い合わせてください。\nhttps://tsubasamusu.com");

                var hashtable2 = new ExitGames.Client.Photon.Hashtable
                {
                    ["IsSettingPicture"] = false
                };

                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable2);

                return;
            }

            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);

            var hashtable = new ExitGames.Client.Photon.Hashtable
            {
                ["PictureBites"] = bytes
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

            uiManagerMain.SetImgBlackBordSprite(sprite, texture.width, texture.height);

            var hashtable1 = new ExitGames.Client.Photon.Hashtable
            {
                ["IsSettingPicture"] = false
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable1);
        }
    }
}