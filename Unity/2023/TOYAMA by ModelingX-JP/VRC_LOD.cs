using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRC_LOD
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VRC_LOD : UdonSharpBehaviour
    {
        [SerializeField, Header("ローポリ")]
        private GameObject objLowPrefab;

        [SerializeField, Header("ハイポリ")]
        private GameObject objHighPrefab;

        [SerializeField, Header("ローポリとハイポリの境目（m）")]
        private float boundaryLength;

        private GameObject objCurrentPrefab;

        private bool isHigh;

        public void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            GenerateGameObject(objLowPrefab);

            isHigh = false;
        }

        public void Update()
        {
            float lengthFromPlayer = (transform.position - Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position).magnitude;

            if (lengthFromPlayer <= boundaryLength && !isHigh)
            {
                GenerateGameObject(objHighPrefab);

                isHigh = true;

                return;
            }

            if (lengthFromPlayer > boundaryLength && isHigh)
            {
                GenerateGameObject(objLowPrefab);

                isHigh = false;
            }
        }

        private void GenerateGameObject(GameObject objPrefab)
        {
            if (objPrefab == null) return;

            if (objCurrentPrefab != null) Destroy(objCurrentPrefab);

            objCurrentPrefab = VRCInstantiate(objPrefab);

            objCurrentPrefab.transform.SetParent(transform);

            objCurrentPrefab.transform.localPosition = objCurrentPrefab.transform.localEulerAngles = Vector3.zero;

            objCurrentPrefab.transform.localScale = Vector3.one;
        }
    }
}