using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class UiManager_Roulette : UiManagerBase
    {
        [SerializeField]
        private CanvasGroup cgRoulette;

        [SerializeField]
        private TextMeshProUGUI tmpError;

        [SerializeField]
        private RectTransform membersParentTran;

        [SerializeField]
        private RectTransform btnSaveTran;

        [SerializeField]
        private RectTransform diskPiecesParentTran;

        [SerializeField]
        private CgLoadingController cgLoadingController;

        [SerializeField]
        private ButtonController saveButtonController;

        [SerializeField]
        private ButtonController controlDiskButtonController;

        [SerializeField]
        private TglMemberController TglMemberPrefab;

        [SerializeField]
        private DiskPieceController DiskPiecePrefab;

        private CancellationTokenSource ctsDiskRotation;

        private List<TglMemberController> tglMemberControllers = new();

        private List<DiskPieceController> diskPieceControllers = new();

        private List<TglMemberController> ValidTglMemberControllers
        {
            get => tglMemberControllers.FindAll(tglMemberController => tglMemberController.IsValid);
        }

        private bool DiskIsRotating
        {
            get => ctsDiskRotation != null;
        }

        private bool startedDiskRotationSpeedReduction;

        private float rotationAnglePerFlames;

        protected override void OnLoadedClass()
        {
            tmpError.text = string.Empty;

            controlDiskButtonController.ButtonText = ConstData.BUTTON_TEXT_START;

            cgRoulette.alpha = 0f;

            saveButtonController.ButtonText = GameData.Instance.Logined ? ConstData.BUTTON_TEXT_SAVE : ConstData.BUTTON_TEXT_MAKE_SAVE_DATA;

            btnSaveTran.sizeDelta = new(GameData.Instance.Logined ? ConstData.WIDTH_BUTTON_SAVE : ConstData.WIDTH_BUTTON_MAKE_SAVE_DATA, btnSaveTran.sizeDelta.y);

            saveButtonController.CgButton.interactable = false;

            saveButtonController.CgButton.alpha = 0f;

            GenerateTglMembers();

            ReGenerateDiskPieces();

            cgRoulette.interactable = false;

            cgRoulette.DOFade(1f, ConstData.TIME_FADE)
                .OnComplete(() =>
                {
                    cgRoulette.interactable = true;
                });
        }

        private void GenerateTglMembers()
        {
            int childCount = membersParentTran.childCount;

            for (int i = 0; i < childCount; i++) Destroy(membersParentTran.GetChild(i).gameObject);

            foreach (Member member in GameData.Instance.LoginedData.members)
            {
                TglMemberController generatedTglMemberController = Instantiate(TglMemberPrefab, membersParentTran);

                tglMemberControllers.Add(generatedTglMemberController);

                generatedTglMemberController.Setup(this, member.name, member.isValid);
            }
        }

        public void OnClickedBtnControlDisk()
        {
            tmpError.text = string.Empty;

            if (!DiskIsRotating)
            {
                ctsDiskRotation = new();

                StartDiskRotationAsync(ctsDiskRotation.Token).Forget();

                DestroyAllTglMembers();

                controlDiskButtonController.ButtonText = ConstData.BUTTON_TEXT_STOP;

                controlDiskButtonController.RestoreButtonColorSchemeAsync(this.GetCancellationTokenOnDestroy()).Forget();

                return;
            }

            startedDiskRotationSpeedReduction = true;

            controlDiskButtonController.Interactable = false;

            controlDiskButtonController.CgButton.DOFade(0f, ConstData.TIME_FADE)
                .OnComplete(() => Destroy(controlDiskButtonController.gameObject));
        }

        private async UniTaskVoid StartDiskRotationAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                rotationAnglePerFlames = startedDiskRotationSpeedReduction ? rotationAnglePerFlames * ConstData.RATIO_DISK_ROTATION_SPEED_REDUCTION_PER_FRAMES : ConstData.ANGLE_ROTATE_DISK_PER_SECONDS * Time.deltaTime;

                if (rotationAnglePerFlames <= 0.01f) OnStoppedDiskRotation();

                foreach (DiskPieceController diskPieceController in diskPieceControllers) diskPieceController.RotateDiskPiece(-rotationAnglePerFlames);

                await UniTask.Yield(cancellationToken);
            }
        }

        private void OnStoppedDiskRotation()
        {
            ctsDiskRotation.Cancel();

            DiskPieceController winningDiskPiece = diskPieceControllers.Find(diskPieceController => diskPieceController.IsWinningDiskPiece());

            winningDiskPiece.SetUiColorsWinning();

            saveButtonController.CgButton.DOFade(1f, ConstData.TIME_FADE)
                .OnComplete(() => saveButtonController.CgButton.interactable = true);

            List<Member> members = GameData.Instance.LoginedData.members;

            foreach (DiskPieceController diskPieceController in diskPieceControllers)
            {
                members.Find(member => member.name == diskPieceController.MemberName).isValid = true;
            }

            members.Find(member => member.name == winningDiskPiece.MemberName).isValid = false;

            GameData.Instance.LoginedData = new()
            {
                saveDataName = GameData.Instance.LoginedData == null ? string.Empty : GameData.Instance.LoginedData.saveDataName,
                passcode = GameData.Instance.LoginedData == null ? string.Empty : GameData.Instance.LoginedData.passcode,
                members = members
            };
        }

        public async void OnClickedBtnSave()
        {
            if (!GameData.Instance.Logined)
            {
                TransitionToNextScene(ConstData.SCENE_NAME_MAKE_SAVE_DATA);

                return;
            }

            cgLoadingController.StartLoadingAnimation();

            saveButtonController.ButtonText = ConstData.BUTTON_TEXT_SAVING;

            SaveToDataBaseResult result = await DataBaseManager.TryOverwriteSaveDataAsync(GameData.Instance.LoginedData);

            cgLoadingController.StopLoadingAnimation();

            if (result == SaveToDataBaseResult.Error)
            {
                saveButtonController.ButtonText = ConstData.BUTTON_TEXT_ERROR;

                return;
            }

            if (result == SaveToDataBaseResult.Success)
            {
                saveButtonController.ButtonText = ConstData.BUTTON_TEXT_SUCCESS;

                TransitionToNextScene(ConstData.SCENE_NAME_TITLE);

                return;
            }

            saveButtonController.ButtonText = ConstData.BUTTON_TEXT_ERROR;

            cgRoulette.interactable = false;

            ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_SAVE_DATA);
        }

        private void TransitionToNextScene(string nextSceneName)
        {
            cgRoulette.interactable = false;

            cgRoulette.DOFade(0f, ConstData.TIME_FADE)
                .OnComplete(() => SceneManager.LoadScene(nextSceneName));
        }

        public void OnClickedTglMember(TglMemberController changedValueTglMemberController)
        {
            if (ValidTglMemberControllers.Count > 1 || string.IsNullOrEmpty(tmpError.text)) tmpError.text = string.Empty;

            if (ValidTglMemberControllers.Count == 0)
            {
                tmpError.text = ConstData.ERROR_NEED_LEAST_ONE_MEMBER;

                changedValueTglMemberController.IsValid = true;

                return;
            }

            ReGenerateDiskPieces();
        }

        private void ReGenerateDiskPieces()
        {
            int childCount = diskPiecesParentTran.childCount;

            for (int i = 0; i < childCount; i++) Destroy(diskPiecesParentTran.GetChild(i).gameObject);

            diskPieceControllers.Clear();

            for (int i = 0; i < ValidTglMemberControllers.Count; i++)
            {
                DiskPieceController generatedDiskPieceController = Instantiate(DiskPiecePrefab, diskPiecesParentTran);

                generatedDiskPieceController.Setup(ValidTglMemberControllers.Count, i, ValidTglMemberControllers[i].MemberName);

                diskPieceControllers.Add(generatedDiskPieceController);
            }
        }

        private void DestroyAllTglMembers()
        {
            foreach (TglMemberController tglMemberController in tglMemberControllers)
            {
                tglMemberController.Interactable = false;

                tglMemberController.CgTglMember.DOFade(0f, ConstData.TIME_FADE)
                    .OnComplete(() =>
                    {
                        Destroy(tglMemberController.gameObject);

                        tglMemberControllers.Remove(tglMemberController);
                    });
            }
        }
    }
}