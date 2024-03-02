using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace yamap
{
    public enum PlayerStormState
    {
        InStorm,
        OutStorm,
    }

    public class StormController : MonoBehaviour
    {
        [SerializeField]
        private float timeLimit;

        [SerializeField]
        private Skybox skybox;

        [SerializeField]
        private Material normalSky;

        [SerializeField]
        private Material stormSky;

        [SerializeField, Header("1秒あたりに受けるストームによるダメージ")]
        private float stormDamage;

        public float StormDamage
        {
            get
            {
                return stormDamage;
            }
        }

        private Vector3 firstStormScale;

        private float currentScaleRate = 100f;

        private void Start()
        {
            firstStormScale = transform.localScale;

            MakeStormSmaller();
        }

        private void MakeStormSmaller()
        {
            DOTween.To(() => currentScaleRate, (x) => currentScaleRate = x, 0f, timeLimit).SetEase(Ease.Linear);
        }

        private void Update()
        {
            transform.localScale = new Vector3((firstStormScale.x * (currentScaleRate / 100f)), firstStormScale.y, (firstStormScale.z * (currentScaleRate / 100f)));
        }

        public bool CheckEnshrine(Vector3 myPos)
        {
            Vector3 pos = Vector3.Scale(myPos, new Vector3(1f, 0f, 1f));

            Vector3 centerPos = Vector3.zero;

            float length = (pos - centerPos).magnitude;

            return length <= transform.localScale.x / 2f ? true : false;
        }

        public void ChangeSkyBox(PlayerStormState playerStormState)
        {
            skybox.material = GetMaterialFromStormState(playerStormState);

            Material GetMaterialFromStormState(PlayerStormState playerStormState)
            {
                return playerStormState == PlayerStormState.InStorm ? stormSky : normalSky;
            }
        }
    }
}