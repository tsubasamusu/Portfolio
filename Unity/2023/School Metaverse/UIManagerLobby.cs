using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SchoolMetaverse
{
    public class UIManagerLobby : MonoBehaviour
    {
        [SerializeField]
        private Text txtPlaceholder;

        [SerializeField]
        private Button btnMain;

        [SerializeField]
        private Button btnSub;

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Image imgLoad;

        [SerializeField]
        private PhotonController photonController;

        private void Start()
        {
            bool isEnterNameScene = false;

            txtPlaceholder.text = "パスコードを入力...";

            imgLoad.DOFade(0f, 0f);

            btnMain.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (isEnterNameScene) return;

                    if (inputField.text != ConstData.PASSCODE)
                    {
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                        return;
                    }

                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    GoToEnterNameScene();
                });

            btnSub.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                    GoToEnterNameScene();
                })
                .AddTo(btnSub);

            void GoToEnterNameScene()
            {
                isEnterNameScene = true;

                Destroy(btnSub.gameObject);

                inputField.text = GameData.instance.playerName;

                txtPlaceholder.text = "名前を入力...";

                btnMain.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        if (inputField.text == string.Empty)
                        {
                            SoundManager.instance.PlaySound(SoundDataSO.SoundName.無効なボタンを押した時の音);

                            return;
                        }

                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.ボタンを押した時の音);

                        GameData.instance.playerName = inputField.text;

                        GameData.instance.SavePlayerNameInDevice();

                        Destroy(inputField.gameObject);

                        Destroy(btnMain.gameObject);

                        StartCoroutine(GoToMain());
                    })
                    .AddTo(btnMain);
            }

            IEnumerator GoToMain()
            {
                photonController.ConnectMasterServer();

                PlayLoadAnimation(this.GetCancellationTokenOnDestroy()).Forget();

                yield return new WaitUntil(() => photonController.JoinedRoom);

                SceneManager.LoadScene("Main");
            }

            async UniTaskVoid PlayLoadAnimation(CancellationToken token)
            {
                imgLoad.DOFade(1f, 0f);

                while (true)
                {
                    imgLoad.transform.Rotate(0f, 0f, -360f / 8f);

                    await UniTask.Delay(TimeSpan.FromSeconds(ConstData.IMG_LOAD_ROT_SPAN), cancellationToken: token);
                }
            }
        }
    }
}