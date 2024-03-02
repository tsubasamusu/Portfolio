using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;

namespace LaserPointer
{
    [RequireComponent(typeof(CapsuleCollider), typeof(VRCPickup), typeof(VRCObjectSync)), UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LaserPointerManager : UdonSharpBehaviour
    {
        [SerializeField]
        private LaserPointerController laserPointerController;

        public override void OnPickupUseDown()
        {
            laserPointerController.OnPickupAndUseDown();
        }
    }
}