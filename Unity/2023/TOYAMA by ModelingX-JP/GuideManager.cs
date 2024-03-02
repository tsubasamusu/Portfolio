using UdonSharp;
using UnityEngine;

namespace Guide
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GuideManager : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject guideNpcControllerPrefab;

        [SerializeField]
        private float displayMessageLength = 5f;

        [SerializeField]
        private GameObject[] objGuideNpcs = new GameObject[0];

        [SerializeField]
        private string[] messages = new string[0];

        public void Start()
        {
            if (guideNpcControllerPrefab == null) return;

            if (objGuideNpcs.Length != messages.Length) return;

            for (int i = 0; i < objGuideNpcs.Length; i++)
            {
                GameObject objController = VRCInstantiate(guideNpcControllerPrefab);

                objController.transform.SetParent(objGuideNpcs[i].transform);

                objController.transform.localPosition = Vector3.zero;

                objController.GetComponent<GuideNpcController>().SetUpNpc(messages[i], displayMessageLength);
            }
        }
    }
}