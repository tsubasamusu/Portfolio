using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class TglMemberController : MonoBehaviour
    {
        [SerializeField]
        private Toggle tglMember;

        [SerializeField]
        private TextMeshProUGUI tmpMemberName;

        [SerializeField]
        private CanvasGroup cgTglMember;

        private UiManager_Roulette uiManager;

        public bool Interactable
        {
            set => tglMember.interactable = value;
        }

        public bool IsValid
        {
            get => tglMember.isOn;

            set => tglMember.isOn = value;
        }

        public string MemberName
        {
            get => tmpMemberName.text;
        }

        public CanvasGroup CgTglMember
        {
            get => cgTglMember;
        }

        public void Setup(UiManager_Roulette uiManager, string memberName, bool isvalid)
        {
            tmpMemberName.text = memberName;

            tglMember.isOn = isvalid;

            tglMember.onValueChanged.AddListener(currentValue => OnClickedTglMember(currentValue));

            this.uiManager = uiManager;
        }

        public void OnClickedTglMember(bool currentValue)
        {
            if (tglMember.isOn != currentValue) tglMember.isOn = currentValue;

            uiManager.OnClickedTglMember(this);
        }
    }
}