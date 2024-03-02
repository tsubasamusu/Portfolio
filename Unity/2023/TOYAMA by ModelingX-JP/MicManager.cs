using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;

namespace Mic
{
    [RequireComponent(typeof(CapsuleCollider), typeof(VRCPickup), typeof(VRCObjectSync)), UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MicManager : UdonSharpBehaviour
    {
        [SerializeField]
        private MicController micController;

        public override void OnPickup()
        {
            micController.OnPickupMic();
        }

        public override void OnDrop()
        {
            micController.OnDropMic();
        }
    }
}