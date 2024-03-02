using DG.Tweening;
using UnityEngine;

namespace AogiriRoom
{
    public class MovieCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraParentTran;

        public void Start()
        {
            Sequence sequence = DOTween.Sequence();

            transform.SetParent(null);

            transform.position = new Vector3(0f, 1.5f, 4f);

            transform.eulerAngles = new Vector3(-70f, 180f, 0f);

            sequence.Append(transform.DORotate(new Vector3(10f, 180f, 0f), 8f))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(1.5f, 1.5f, 3f);

                    transform.eulerAngles = new Vector3(0f, -90f, 0f);
                })
                .Append(transform.DOMoveZ(-3f, 15f).SetEase(Ease.OutSine))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(1.15f, 0.25f, -1.3f);

                    transform.eulerAngles = new Vector3(0f, -90f, 0f);
                })
                .Append(transform.DOMoveY(1f, 10f))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(-1.8f, 1f, -4.5f);

                    transform.eulerAngles = Vector3.zero;
                })
                .Append(transform.DOMoveX(2.5f, 15f).SetEase(Ease.OutSine))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(2f, 1.6f, -3.46f);

                    transform.eulerAngles = new Vector3(-50f, 90f, 0f);
                })
                .Append(transform.DORotate(new Vector3(0f, 90f, 0f), 7f))
                .AppendCallback(() =>
                {
                    transform.SetParent(null);

                    transform.position = new Vector3(2.5f, 1f, -1f);

                    transform.eulerAngles = new Vector3(0f, 180f, 0f);

                    cameraParentTran.position = new Vector3(2.5f, 1f, -2.3f);

                    cameraParentTran.eulerAngles = Vector3.zero;

                    transform.SetParent(cameraParentTran);
                })
                .Append(cameraParentTran.DORotate(new Vector3(0f, -130f, 0f), 15f).SetEase(Ease.InOutSine))
                .OnComplete(() => transform.SetParent(null))
                .AppendCallback(() =>
                 {
                     transform.position = new Vector3(2.1f, 0.7f, -3.5f);

                     transform.eulerAngles = new Vector3(0f, 90f, 0f);
                 })
                .Append(transform.DOMoveZ(-4.2f, 10f).SetEase(Ease.OutSine))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(2.1f, 1.3f, -3.5f);

                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                })
                .Append(transform.DOMoveX(-1.1f, 10f).SetEase(Ease.OutSine))
                .AppendCallback(() =>
                 {
                     transform.SetParent(null);

                     transform.position = new Vector3(0.005f, 2f, 2.635f);

                     transform.eulerAngles = new Vector3(-45f, 180f, 0f);

                     cameraParentTran.position = new Vector3(0.005f, 2.5f, 2.135f);

                     cameraParentTran.eulerAngles = Vector3.zero;

                     transform.SetParent(cameraParentTran);
                 })
                .Append(cameraParentTran.DORotate(new Vector3(0f, 180f, 0f), 15f).SetEase(Ease.InOutSine))
                .OnComplete(() => transform.SetParent(null))
                .AppendCallback(() =>
                {
                    transform.position = new Vector3(1.4f, 2.8f, 4f);

                    transform.eulerAngles = new Vector3(0f, 200f, 0f);
                })
                .Append(transform.DOMoveY(1f, 10f).SetEase(Ease.OutSine));
        }
    }
}