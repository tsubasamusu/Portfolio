using UnityEngine;
using UnityEngine.UI;

namespace LightingDemonstration
{
    public class PreviewButton : ButtonBase
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Image image;

        protected override void OnInitialized()
        {
            SetEnable(false);
        }

        public void SetEnable(bool enable)
        {
            if (enable)
            {
                image.color = Color.black;

                button.interactable = true;

                return;
            }

            image.color = Color.gray;

            button.interactable = false;
        }
    }
}