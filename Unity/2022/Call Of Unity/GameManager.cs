using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TNRD;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CallOfUnity
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private List<SerializableInterface<ISetUp>> iSetUpList0 = new();

        [SerializeField]
        private List<SerializableInterface<ISetUp>> iSetUpList1 = new();

        private void Start()
        {
            SetUp(0);

            GameData.instance.UiManager.EndedGameStartPerformance
                .Where(_ => GameData.instance.UiManager.EndedGameStartPerformance.Value == true)
                .Subscribe(_ => StartGame())
                .AddTo(this);

            GameData.instance.Score
                .Subscribe(_ =>
                {
                    if (GameData.instance.Score.Value.team0 >= ConstData.WIN_SCORE) EndGame(true);

                    if (GameData.instance.Score.Value.team1 >= ConstData.WIN_SCORE) EndGame(false);
                })
                .AddTo(this);

            void StartGame()
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.試合中のBGM, ConstData.BGM_VOLUME, true);

                Cursor.visible = !GameData.instance.hideMouseCursor;

                SetUp(1);
            }

            void SetUp(int setUpNo)
            {
                for (int i = 0; i < (setUpNo == 0 ? iSetUpList0 : iSetUpList1).Count; i++)
                {
                    (setUpNo == 0 ? iSetUpList0 : iSetUpList1)[i].Value.SetUp();
                }
            }

            void EndGame(bool isGameClear)
            {
                Camera.main.transform.parent = null;

                Destroy(GameData.instance.TemporaryObjectContainerTran.gameObject);

                while (GameData.instance.npcControllerBaseList.Count > 0)
                {
                    Destroy(GameData.instance.npcControllerBaseList[0].gameObject);

                    GameData.instance.npcControllerBaseList.RemoveAt(0);
                }

                Destroy(GameData.instance.PlayerControllerBase.gameObject);

                if (!Cursor.visible) Cursor.visible = true;

                GameData.instance.SaveData();

                SoundManager.instance.StopSound();

                SoundManager.instance.PlaySound(isGameClear ? SoundDataSO.SoundName.ゲームクリアの音 : SoundDataSO.SoundName.ゲームオーバーの音);

                GameData.instance.UiManager.PlayGameEndPerformance(isGameClear);

                GameData.instance.UiManager.EndedGameEndPerformance
                    .Where(_ => GameData.instance.UiManager.EndedGameEndPerformance.Value == true)
                    .Subscribe(_ => SceneManager.LoadScene("Main"))
                    .AddTo(this);
            }
        }
    }
}