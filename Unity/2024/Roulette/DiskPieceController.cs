using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class DiskPieceController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform diskPieceTran;

        [SerializeField]
        private RectTransform imgDiskPieceTran;

        [SerializeField]
        private Image imgDiskPiece;

        [SerializeField]
        private TextMeshProUGUI tmpMemberName;

        private int validMembersCount;

        private float diskPieceLocalAngleZ;

        private float DiskPieceLocalAngleZ
        {
            get => Mathf.Abs(diskPieceLocalAngleZ) % 360f;

            set
            {
                diskPieceLocalAngleZ = value;

                diskPieceTran.localEulerAngles = new(0f, 0f, value);
            }
        }

        public string MemberName
        {
            get => tmpMemberName.text;
        }

        public void Setup(int currentValidMembersCount, int thisDiskPieceIndex, string memberName)
        {
            validMembersCount = currentValidMembersCount;

            tmpMemberName.text = memberName;

            SetUiColorsDefault();

            UpdateDiskPieceLocalAngleZ(currentValidMembersCount, thisDiskPieceIndex);

            UpdateImgDiskPieceAngle(currentValidMembersCount);

            UpdateImgDiskFillAmount(currentValidMembersCount);

            UpdateImgDiskPieceWidth();
        }

        private void UpdateImgDiskPieceAngle(int currentValidMembersCount) => imgDiskPieceTran.localEulerAngles = new(0f, 0f, -180f + (GetDiskPieceInternalAngle(currentValidMembersCount) / 2));

        private void UpdateImgDiskFillAmount(int currentValidMembersCount) => imgDiskPiece.fillAmount = 1f / currentValidMembersCount;

        private void UpdateImgDiskPieceWidth() => imgDiskPieceTran.sizeDelta = new(imgDiskPieceTran.sizeDelta.y, imgDiskPieceTran.sizeDelta.y);

        private void UpdateDiskPieceLocalAngleZ(int currentValidMembersCount, int thisDiskPieceIndex) => DiskPieceLocalAngleZ = -(GetDiskPieceInternalAngle(currentValidMembersCount) * thisDiskPieceIndex);

        private float GetDiskPieceInternalAngle(int currentValidMembersCount) => 360f / currentValidMembersCount;

        private void SetUiColorsDefault()
        {
            imgDiskPiece.color = Color.white;

            tmpMemberName.color = Color.black;
        }

        public void SetUiColorsWinning()
        {
            imgDiskPiece.color = Color.red;

            tmpMemberName.color = Color.white;
        }

        public void RotateDiskPiece(float rotateValue)
        {
            diskPieceTran.Rotate(0f, 0f, rotateValue);

            diskPieceLocalAngleZ += rotateValue;

            tmpMemberName.transform.localEulerAngles = new(0f, 0f, DiskPieceLocalAngleZ >= 180f ? -90f : 90f);
        }

        public bool IsWinningDiskPiece() => DiskPieceLocalAngleZ >= (360f - (GetDiskPieceInternalAngle(validMembersCount) / 2f)) || DiskPieceLocalAngleZ <= (GetDiskPieceInternalAngle(validMembersCount) / 2f);
    }
}