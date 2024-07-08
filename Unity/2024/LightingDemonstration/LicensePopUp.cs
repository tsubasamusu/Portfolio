using UnityEngine;

namespace LightingDemonstration
{
    public class LicensePopUp : MonoBehaviour, ISetup
    {
        [SerializeField]
        private CanvasGroup cgLicensePopUp;

        [SerializeField]
        private Joystick joystick;

        [SerializeField]
        private TextBase licenseContentText;

        public void Setup()
        {
            HideLicensePopUp();

            licenseContentText.SetText(ConstDataSO.Instance.licenseText.text);
        }

        public void ShowLicensePopUp()
        {
            joystick.SetEnable(false);

            cgLicensePopUp.alpha = 1f;

            cgLicensePopUp.blocksRaycasts = true;

            cgLicensePopUp.interactable = true;
        }

        public void HideLicensePopUp()
        {
            joystick.SetEnable(true);

            cgLicensePopUp.alpha = 0f;

            cgLicensePopUp.blocksRaycasts = false;

            cgLicensePopUp.interactable = false;
        }
    }
}