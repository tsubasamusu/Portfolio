using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pencil
{
    [RequireComponent(typeof(LineRenderer)), UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LineController : UdonSharpBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField, Range(3, 100)]
        private int maxLinePositionsCount = 3;

        [SerializeField, Range(0f, 0.1f)]
        private float minLengthFromEraser = 0.05f;

        private PencilController pencilController;

        private PencilModeController pencilModeController;

        private Transform pencilTran;

        public int MaxLinePositionsCount { get => maxLinePositionsCount; }

        public void SetUpLine(PencilController pencilController, PencilModeController pencilModeController, Transform pencilTran, Color lineColor)
        {
            this.pencilController = pencilController;

            this.pencilModeController = pencilModeController;

            this.pencilTran = pencilTran;

            lineRenderer.startColor = lineRenderer.endColor = lineColor;
        }

        public void AddNewLinePosition(Vector3 addValue)
        {
            Vector3[] linePositionsBefore = new Vector3[0];

            if (lineRenderer.positionCount > 0)
            {
                linePositionsBefore = new Vector3[lineRenderer.positionCount];

                lineRenderer.GetPositions(linePositionsBefore);
            }

            lineRenderer.positionCount++;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                if (i == lineRenderer.positionCount - 1)
                {
                    lineRenderer.SetPosition(i, addValue);

                    break;
                }

                lineRenderer.SetPosition(i, linePositionsBefore[i]);
            }

            pencilController.AddSyncLinePositions(addValue);
        }

        private void Update()
        {
            if (pencilController == null) return;

            if (!Networking.IsOwner(pencilController.gameObject)) return;

            if (pencilModeController == null) return;

            if (!pencilModeController.SyncIsEraserMode) return;

            if (!GetEraserIsNear()) return;

            pencilController.ResetSyncLinePositions(this);
        }

        private bool GetEraserIsNear()
        {
            if (pencilTran == null || lineRenderer == null) return false;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                if ((lineRenderer.GetPosition(i) - pencilTran.position).magnitude <= minLengthFromEraser) return true;
            }

            return false;
        }

        public bool GetIsMaxLinePositions() => lineRenderer.positionCount == maxLinePositionsCount;

        public int GetLinePositionCount() => lineRenderer.positionCount;

        public void ResetLinePositions()
        {
            lineRenderer.positionCount = 0;
        }

        public LineRenderer GetLineRenderer() => lineRenderer;

        public Vector3 GetLastLinePosition()
        {
            if (lineRenderer.positionCount == 0) return Vector3.zero;

            return lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        }
    }
}