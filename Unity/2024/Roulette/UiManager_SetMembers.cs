using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class UiManager_SetMembers : UiManagerBase
    {
        [SerializeField]
        private CanvasGroup cgSetMembers;

        [SerializeField]
        private RectTransform membersParentTran;

        [SerializeField]
        private TextMeshProUGUI tmpError;

        [SerializeField]
        private IfMemberNameController ifMemberNamePrefab;

        [SerializeField]
        private ButtonController nextButtonController;

        [SerializeField]
        private ButtonController addMemberButtonController;

        [SerializeField]
        private ButtonController logoutButtonController;

        private List<IfMemberNameController> ifMemberNameControllers = new();

        protected override void OnLoadedClass()
        {
            cgSetMembers.interactable = false;

            tmpError.text = string.Empty;

            cgSetMembers.alpha = 0f;

            logoutButtonController.ButtonText = GameData.Instance.Logined ? ConstData.BUTTON_TEXT_LOGOUT : ConstData.BUTTON_TEXT_BACK;

            GenerateIfMemberNames();

            cgSetMembers.DOFade(1f, ConstData.TIME_FADE).OnComplete(() => cgSetMembers.interactable = true);
        }

        private void GenerateIfMemberNames()
        {
            ClearChildsOfMembersParent();

            if (!GameData.Instance.Logined)
            {
                AddIfMemberName();

                return;
            }

            if (GameData.Instance.LoginedData == null || GameData.Instance.LoginedData.members == null || GameData.Instance.LoginedData.members.Count == 0)
            {
                AddIfMemberName();

                return;
            }

            int allMembersCount = GameData.Instance.LoginedData.members.Count;

            for (int i = 0; i < allMembersCount; i++)
            {
                Member member = GameData.Instance.LoginedData.members[i];

                AddIfMemberName(member.name, member.isValid);
            }
        }

        private void ClearChildsOfMembersParent()
        {
            List<GameObject> gameObjectsToDelete = new();

            for (int i = 0; i < membersParentTran.childCount; i++)
            {
                Transform childTran = membersParentTran.GetChild(i);

                if (childTran.GetChild(0).TryGetComponent(out ButtonController buttonController) && buttonController == addMemberButtonController) continue;

                gameObjectsToDelete.Add(childTran.gameObject);
            }

            foreach (GameObject gameObjectToDelete in gameObjectsToDelete) Destroy(gameObjectToDelete);
        }

        public void OnEnteredMemberName()
        {
            if (tmpError.text == ConstData.ERROR_NO_NAME_MEMBER) tmpError.text = string.Empty;
        }

        public void DeleteIfMemberName(IfMemberNameController ifMemberNameController)
        {
            if (ifMemberNameControllers.Count == 1)
            {
                tmpError.text = ConstData.ERROR_NEED_LEAST_ONE_MEMBER;

                return;
            }

            if (tmpError.text == ConstData.ERROR_CANNOT_ADD_MORE_MEMBERS) tmpError.text = string.Empty;

            if (ifMemberNameControllers.Find(member => member == ifMemberNameController) != null) ifMemberNameControllers.Remove(ifMemberNameController);

            Destroy(ifMemberNameController.gameObject);

            if (!addMemberButtonController.transform.parent.gameObject.activeSelf) addMemberButtonController.transform.parent.gameObject.SetActive(true);
        }

        public void AddIfMemberName(string memberName = "", bool isValid = true)
        {
            if (ifMemberNameControllers.Count == ConstData.MAX_MAMBERS_COUNT)
            {
                tmpError.text = ConstData.ERROR_CANNOT_ADD_MORE_MEMBERS;

                return;
            }

            if (ifMemberNameControllers.Count == ConstData.MAX_MAMBERS_COUNT - 1) addMemberButtonController.transform.parent.gameObject.SetActive(false);

            IfMemberNameController generatedIfMemberNameController = Instantiate(ifMemberNamePrefab, membersParentTran);

            ifMemberNameControllers.Add(generatedIfMemberNameController);

            generatedIfMemberNameController.Setup(this, memberName, isValid);

            addMemberButtonController.transform.parent.SetAsLastSibling();
        }

        public void OnClickedBtnNext()
        {
            tmpError.text = string.Empty;

            List<Member> members = new();

            bool enteredNoMemberName = false;

            foreach (IfMemberNameController ifMemberNameController in ifMemberNameControllers)
            {
                if (string.IsNullOrEmpty(ifMemberNameController.EnteredMemberName))
                {
                    enteredNoMemberName = true;

                    ifMemberNameController.TmpPlaceholderColor = Color.red;
                }

                if (enteredNoMemberName) continue;

                Member member = new()
                {
                    name = ifMemberNameController.EnteredMemberName,
                    isValid = ifMemberNameController.IsValid,
                };

                members.Add(member);
            }

            if (enteredNoMemberName)
            {
                tmpError.text = ConstData.ERROR_NO_NAME_MEMBER;

                nextButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            GameData.Instance.LoginedData = new()
            {
                saveDataName = GameData.Instance.LoginedData == null ? string.Empty : GameData.Instance.LoginedData.saveDataName,
                passcode = GameData.Instance.LoginedData == null ? string.Empty : GameData.Instance.LoginedData.passcode,
                members = members
            };

            TransitionToNextScene(ConstData.SCENE_NAME_ROULETTE);
        }

        public void OnClickedBtnLogout()
        {
            GameData.Instance.DeleteStoredSaveData();

            GameData.Instance.LoginedData = null;

            TransitionToNextScene(ConstData.SCENE_NAME_LOGIN);
        }

        public void OnClickedBtnAddMember()
        {
            if (tmpError.text == ConstData.ERROR_NEED_LEAST_ONE_MEMBER) tmpError.text = string.Empty;

            AddIfMemberName();
        }

        private void TransitionToNextScene(string nextSceneName)
        {
            cgSetMembers.interactable = false;

            cgSetMembers.DOFade(0f, ConstData.TIME_FADE).OnComplete(() => SceneManager.LoadScene(nextSceneName));
        }
    }
}