using UnityEngine;

namespace LightingDemonstration
{
    public class ButtonsParent : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup cgButtons;

        public void SetEnable(bool enable, bool changeVisibility = false)
        {
            if (changeVisibility) cgButtons.alpha = enable ? 1 : 0;

            cgButtons.blocksRaycasts = cgButtons.interactable = enable;
        }
    }
}