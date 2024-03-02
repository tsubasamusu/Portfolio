using TMPro;
using UnityEngine;

namespace Roulette
{
    public class IfMemberNameController : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField ifMemberName;

        [SerializeField]
        private TextMeshProUGUI tmpPlaceholder;

        [SerializeField]
        private ButtonController deleteMemberButtonController;

        private Color defaultTmpPlaceholderColor;

        private UiManager_SetMembers uiManager;

        private bool isValid;

        public Color TmpPlaceholderColor
        {
            set => tmpPlaceholder.color = value;
        }

        public string EnteredMemberName
        {
            get => ifMemberName.text;
        }

        public bool IsValid
        {
            get => isValid;
        }

        public void Setup(UiManager_SetMembers uiManager, string memberName, bool isValid)
        {
            ifMemberName.onValueChanged.AddListener(enteredMemberName => OnEnteredMemberName(enteredMemberName));

            defaultTmpPlaceholderColor = tmpPlaceholder.color;

            ifMemberName.text = memberName;

            deleteMemberButtonController.Setup();

            deleteMemberButtonController.ImageSprite = CloudStorageData.Instance.GetSpriteFromTextureType(TextureType.TrashCan);

            this.uiManager = uiManager;

            this.isValid = isValid;
        }

        private void OnEnteredMemberName(string enteredMemberName)
        {
            if (enteredMemberName.Length > ConstData.MEX_LENGTH_MEMBER_NAME)
            {
                ifMemberName.text = enteredMemberName[..ConstData.MEX_LENGTH_MEMBER_NAME];

                return;
            }

            if (uiManager == null) return;

            uiManager.OnEnteredMemberName();

            if (tmpPlaceholder.color != defaultTmpPlaceholderColor) tmpPlaceholder.color = defaultTmpPlaceholderColor;

            ifMemberName.text = DataBaseManager.ConvertAvailableText(enteredMemberName);
        }

        public void OnClickedBtnDeleteMember()
        {
            if (uiManager == null) return;

            uiManager.DeleteIfMemberName(this);
        }
    }
}