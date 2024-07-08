using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LightingDemonstration
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class HouseButtonsGridLayoutGroup : MonoBehaviour, ISetup
    {
        [SerializeField]
        private GridLayoutGroup gridLayoutGroup;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private UiManager_Home uiManager;

        public void Setup()
        {
            DestroyAllChildren();

            GenerateHouseButtons();

            this.ObserveEveryValueChanged(_ => GetSelfWidth())
                .Subscribe(_ => UpdateAllCellSize())
                .AddTo(this);
        }

        private void DestroyAllChildren()
        {
            for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
        }

        private void GenerateHouseButtons()
        {
            foreach (HouseData houseData in ConstDataSO.Instance.houseDatas)
            {
                HouseButton houseButton = Instantiate(ConstDataSO.Instance.houseButtonPrefab);

                houseButton.transform.SetParent(transform);

                houseButton.Setup(houseData, uiManager);

                uiManager.houseButtons.Add(houseButton);
            }
        }

        private void UpdateAllCellSize()
        {
            int columnCount = gridLayoutGroup.constraintCount;

            float desiredCellSize = (GetSelfWidth() - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) - (gridLayoutGroup.spacing.x * (columnCount - 1))) / columnCount;

            gridLayoutGroup.cellSize = new(desiredCellSize, desiredCellSize);
        }

        private float GetSelfWidth()
        {
            Vector3[] cornerWorldPositions = new Vector3[4];

            rectTransform.GetWorldCorners(cornerWorldPositions);

            float selfWidth = cornerWorldPositions[3].x - cornerWorldPositions[0].x;

            return selfWidth;
        }
    }
}