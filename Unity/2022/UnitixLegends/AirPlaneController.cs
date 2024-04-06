using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace yamap
{
    public class AirPlaneController : MonoBehaviour
    {
        [SerializeField]
        private Transform propellerTran;

        [SerializeField]
        private Transform aiplanePlayerTran;

        [SerializeField]
        private CinemachineManager cinemachineManager;

        [SerializeField]
        private KeyCode fallKey;

        [SerializeField]
        private float rotSpeed;

        private bool fellFromAirplane;

        private bool endFight;

        public void SetUpAirplane()
        {
            transform.position = new Vector3(120f, 100f, -120f);

            StartCoroutine(RotatePropeller());

            StartCoroutine(NavigateAirplane());

            cinemachineManager.SetPlayerCharacterActive(false);

            SoundManager.instance.PlaySE(SeName.AirplaneSE, true);
        }

        public IEnumerator ControlPlayerMovement(GameManager gameManager, PlayerController player)
        {
            while (!fellFromAirplane)
            {
                player.transform.position = aiplanePlayerTran.position;

                if (endFight)
                {
                    StartCoroutine(FallFromAirplane());

                    gameManager.FallAirPlane();
                }

                if (Input.GetKeyDown(fallKey))
                {
                    StartCoroutine(FallFromAirplane());

                    gameManager.FallAirPlane();
                }

                yield return null;
            }
        }

        private IEnumerator FallFromAirplane()
        {
            SoundManager.instance.PlaySE(SeName.FallSE);

            cinemachineManager.SetAirplaneCameraPriority(9);

            fellFromAirplane = true;

            yield return new WaitForSeconds(5f);

            SoundManager.instance.ClearAudioSource();
        }

        private IEnumerator RotatePropeller()
        {
            while (true)
            {
                propellerTran.Rotate(0f, rotSpeed, 0f);

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        private IEnumerator NavigateAirplane()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 pos = Vector3.Scale(transform.forward, new Vector3(240f, 0f, 240f)) + transform.position;

                transform.DOMove(pos, 10f).SetEase(Ease.Linear);

                yield return new WaitForSeconds(10f);

                if (i == 3)
                {
                    break;
                }

                transform.DORotate(new Vector3(0f, (float)-90 * (i + 1), 0f), 1f).SetEase(Ease.Linear);

                yield return new WaitForSeconds(1f);
            }

            endFight = true;

            transform.DOMoveX(transform.position.x + 100f, 10f);

            yield return new WaitForSeconds(10f);

            Destroy(gameObject);
        }
    }
}