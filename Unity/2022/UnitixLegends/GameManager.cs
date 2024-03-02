using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace yamap
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyGenerator enemyGenerator;

        [SerializeField]
        private AirPlaneController airplaneController;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private UIManager uiManager;

        [SerializeField]
        private StormController stormController;

        [SerializeField]
        private BulletManager bulletManager;

        [SerializeField]
        private Transform temporaryObjectContainerTran;

        private bool isGameOver;

        public bool IsGameOver
        {
            get
            {
                return isGameOver;
            }
        }

        private IEnumerator Start()
        {
            SoundManager.instance.PlaySE(SeName.GameStartSE);

            playerController.enabled = false;

            playerController.SetUpPlayer(uiManager);

            uiManager.SetUpUIManager(bulletManager, playerController.PlayerHealth);

            uiManager.SetCanvasGroup(false);

            uiManager.SetMessageActive(false);

            bulletManager.SetUpBulletManager(playerController, temporaryObjectContainerTran);

            ItemManager.instance.SetUpItemManager(uiManager, playerController, bulletManager);

            GameData.instance.KillCount = 0;

            yield return uiManager.PlayGameStart();

            uiManager.SetMessageActive(true);

            airplaneController.SetUpAirplane();

            uiManager.SetMessageText("Tap\n'Space'\nTo Fall", Color.blue);

            StartCoroutine(airplaneController.ControlPlayerMovement(this, playerController));

            StartCoroutine(enemyGenerator.GenerateEnemy(uiManager, playerController));

            uiManager.SetUpItemSlots();

            StartCoroutine(CheckStormDamageAsync());
        }

        public void FallAirPlane()
        {
            uiManager.SetMessageText("", Color.black);

            playerController.enabled = true;

            uiManager.SetCanvasGroup(true);

            StartCoroutine(uiManager.UpdateText());
        }

        private IEnumerator CheckStormDamageAsync()
        {
            bool skyFlag = false;

            while (playerController.PlayerHealth.PlayerHp > 0)
            {
                while (!stormController.CheckEnshrine(playerController.transform.position))
                {
                    if (skyFlag)
                    {
                        stormController.ChangeSkyBox(PlayerStormState.InStorm);

                        skyFlag = false;
                    }

                    playerController.PlayerHealth.UpdatePlayerHp(-stormController.StormDamage);

                    yield return new WaitForSeconds(1f);

                    if (playerController.PlayerHealth.PlayerHp <= 0)
                    {
                        break;
                    }
                }

                if (!skyFlag)
                {
                    stormController.ChangeSkyBox(PlayerStormState.OutStorm);

                    skyFlag = true;
                }

                yield return null;
            }

            StartCoroutine(MakeGameOverAsync());
        }

        public IEnumerator MakeGameClear()
        {
            SoundManager.instance.PlaySE(SeName.GameClearSE);

            SetUpGameOver();

            yield return StartCoroutine(uiManager.PlayGameClear());

            SceneManager.LoadScene("Main");
        }

        public IEnumerator MakeGameOverAsync()
        {
            SoundManager.instance.PlaySE(SeName.GameOverSE);

            SetUpGameOver();

            yield return StartCoroutine(uiManager.PlayGameOver());

            SceneManager.LoadScene("Main");
        }

        private void SetUpGameOver()
        {
            isGameOver = true;

            uiManager.DisplayGameOver();

            playerController.enabled = false;
        }
    }
}