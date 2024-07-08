using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LightingDemonstration
{
    [RequireComponent(typeof(Button))]
    public class ButtonBase : MonoBehaviour, ISetup
    {
        [SerializeField]
        private bool keepEnabledOnClickedButton = true;

        [SerializeField]
        private bool replaceButtonColorOnClickedButton = true;

        [SerializeField]
        private UnityEvent onClickedEvent = new();

        public void Setup()
        {
            if (!TryGetComponent(out Button button))
            {
                Debug.LogError("Failed to get the Button.");

                return;
            }

            button.onClick.AddListener(() =>
            {
                button.enabled = keepEnabledOnClickedButton;

                if (replaceButtonColorOnClickedButton) ReplaceButtonColor();

                OnClickedButton();

                onClickedEvent.Invoke();
            });

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {

        }

        protected virtual void OnClickedButton()
        {

        }

        private void ReplaceButtonColor()
        {
            if (!TryGetComponent(out Image image))
            {
                Debug.LogError("The Button color could not be changed because of a failure to get the Image.");

                return;
            }

            if (transform.GetChild(0) == null || !transform.GetChild(0).TryGetComponent(out TextMeshProUGUI text))
            {
                Debug.LogError("The Button color could not be changed because of a failure to get the TextMeshProUGUI.");

                return;
            }

            (image.color, text.color) = (text.color, image.color);
        }
    }
}