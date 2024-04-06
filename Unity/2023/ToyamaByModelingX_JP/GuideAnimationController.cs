using UdonSharp;
using UnityEngine;

namespace Guide
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), RequireComponent(typeof(Animator))]
    public class GuideAnimationController : UdonSharpBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private GameObject objStaffMarkerPrefab;

        [SerializeField]
        private Transform headTran;

        [SerializeField]
        private float heightFromHead = 0.3f;

        [SerializeField]
        private float markerRotationPerSecond = 45f;

        private int maxAnimationCount = 5;

        private int currentAnimationNo = -1;

        private Transform generatedStaffMarkerTran;

        public void Start()
        {
            generatedStaffMarkerTran = VRCInstantiate(objStaffMarkerPrefab).transform;
        }

        public void Update()
        {
            ControlStaffMarker();

            if (currentAnimationNo == -1)
            {
                RestartAnimation();

                return;
            }

            if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f) >= 0.9f) RestartAnimation();
        }

        private void ControlStaffMarker()
        {
            if (generatedStaffMarkerTran == null) return;

            generatedStaffMarkerTran.Rotate(0f, markerRotationPerSecond * Time.deltaTime, 0f);

            generatedStaffMarkerTran.position = headTran.position + new Vector3(0f, heightFromHead, 0f);
        }

        public void RestartAnimation()
        {
            if (currentAnimationNo != -1) animator.SetBool(GetParameterName(currentAnimationNo), false);

            currentAnimationNo = GetRandomAnimationNo();

            animator.SetBool(GetParameterName(currentAnimationNo), true);
        }

        private int GetRandomAnimationNo()
        {
            int randomNo = 0;

            while (true)
            {
                if (randomNo != currentAnimationNo) break;

                randomNo = Random.Range(0, maxAnimationCount);
            }

            return randomNo;
        }

        private string GetParameterName(int animationNo) => "Play" + animationNo.ToString();

        public void OnDestroy()
        {
            Destroy(generatedStaffMarkerTran.gameObject);
        }
    }
}