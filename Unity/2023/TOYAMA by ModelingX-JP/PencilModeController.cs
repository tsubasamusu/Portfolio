using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Pencil
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PencilModeController : UdonSharpBehaviour
    {
        [SerializeField]
        private BtnSwitchModeController btnSwitchModeController;

        [SerializeField]
        private PencilController pencilController;

        [SerializeField]
        private CapsuleCollider capsuleCollider;

        [SerializeField]
        private BoxCollider boxCollider;

        [SerializeField]
        private VRCPickup vrcPickup;

        [SerializeField]
        private GameObject objPencilMesh;

        [SerializeField]
        private GameObject objEraserMesh;

        [UdonSynced]
        private bool syncIsEraserMode;

        public bool SyncIsEraserMode
        {
            get => syncIsEraserMode;
        }

        private void Start()
        {
            UpdateMode();
        }

        public override void OnDeserialization()
        {
            UpdateMode();
        }

        private void UpdateMode()
        {
            vrcPickup.UseText = syncIsEraserMode ? string.Empty : "描く";

            btnSwitchModeController.SwitchButtonText(syncIsEraserMode);

            objPencilMesh.SetActive(!syncIsEraserMode);

            objEraserMesh.SetActive(syncIsEraserMode);

            capsuleCollider.enabled = !syncIsEraserMode;

            boxCollider.enabled = syncIsEraserMode;
        }

        public void SwitchMode()
        {
            Networking.SetOwner(Networking.GetOwner(pencilController.gameObject), gameObject);

            if (!Networking.IsOwner(gameObject)) return;

            syncIsEraserMode = !syncIsEraserMode;

            RequestSerialization();

            UpdateMode();
        }
    }
}