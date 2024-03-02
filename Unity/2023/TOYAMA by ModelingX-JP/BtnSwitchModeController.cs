using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Pencil
{
    [RequireComponent(typeof(BoxCollider)), UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BtnSwitchModeController : UdonSharpBehaviour
    {
        [SerializeField]
        private PencilModeController pencilModeController;

        [SerializeField]
        private Text txtButton;

        public override void Interact()
        {
            PrepareSwitchMode();
        }

        public void PrepareSwitchMode()
        {
            pencilModeController.SwitchMode();
        }

        public void SwitchButtonText(bool switchToEraser)
        {
            txtButton.text = "Swtch\nTo\n" + (switchToEraser ? "Pen" : "Eraser");
        }

        private void Update()
        {
            transform.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
        }
    }
}