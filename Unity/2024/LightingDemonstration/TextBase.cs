using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace LightingDemonstration
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
    public class TextBase : MonoBehaviour, ISetup
    {
        [SerializeField]
        private bool fixHeightAutomatically = true;

        private TextMeshProUGUI textMeshPro;

        private RectTransform rectTransform;

        public void Setup()
        {
            if (!TryGetComponent(out textMeshPro))
            {
                Debug.LogError("Failed to get the TextMeshProUGUI.");

                return;
            }

            if (!TryGetComponent(out rectTransform))
            {
                Debug.LogError("Failed to get the RectTransform.");

                return;
            }

            if (!fixHeightAutomatically) return;

            this.UpdateAsObservable()
                .Subscribe(_ => FixHeight())
                .AddTo(this);
        }

        private void FixHeight()
        {
            if (textMeshPro == null || rectTransform == null) return;

            float height = textMeshPro.textInfo.lineCount * ConstDataSO.Instance.textHeightPerSizeOne * textMeshPro.fontSize;

            rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, height);
        }

        public void SetText(string text)
        {
            if (textMeshPro != null) textMeshPro.text = text;
        }
    }
}