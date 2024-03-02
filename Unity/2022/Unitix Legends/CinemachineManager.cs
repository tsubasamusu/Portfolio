using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace yamap
{
    public class CinemachineManager : MonoBehaviour
    {
        [SerializeField]
        private CinemachineFreeLook airplaneCamera;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private Transform miniMapBackgroundTran;

        private Transform mainCameraTran;

        [SerializeField]
        private Transform playerCharacterTran;

        [SerializeField]
        private GameObject playerCharacterbody;

        private float angle;

        private void Start()
        {
            mainCameraTran = Camera.main.transform;

            angle = playerCharacterTran.eulerAngles.y + 90f;
        }

        private void Update()
        {
            if (playerController.enabled == false)
            {
                return;
            }

            miniMapBackgroundTran.position = new Vector3(playerController.transform.position.x, miniMapBackgroundTran.position.y, playerController.transform.position.z);

            bool set = CheckIntercepted() ? false : true;

            SetPlayerCharacterActive(set);
        }

        private bool CheckIntercepted()
        {
            if (mainCameraTran.eulerAngles.y >= angle - 20f && mainCameraTran.eulerAngles.y <= angle + 20f)
            {
                return true;
            }
            else if (mainCameraTran.eulerAngles.y >= (angle + 180f) - 20f && mainCameraTran.eulerAngles.y <= (angle + 180f) + 20f)
            {
                return true;
            }

            return false;
        }

        public void SetAirplaneCameraPriority(int airplaneCameraPriority)
        {
            airplaneCamera.Priority = airplaneCameraPriority;
        }

        public void SetPlayerCharacterActive(bool set)
        {
            playerCharacterbody.SetActive(set);
        }
    }
}