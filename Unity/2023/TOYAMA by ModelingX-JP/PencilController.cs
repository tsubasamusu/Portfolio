using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pencil
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PencilController : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject objPencilLinePrefab;

        [SerializeField]
        private PencilModeController pencilModeController;

        [SerializeField]
        private Transform topOfPencilTran;

        [SerializeField]
        private Transform linesParentTran;

        [SerializeField, Range(0f, 0.1f)]
        private float lengthBetweenLinePos = 0.01f;

        [SerializeField]
        private Color pencilColor = Color.black;

        [UdonSynced]
        private Vector3[] syncLinePositions = new Vector3[0];

        [UdonSynced]
        private bool syncOwnerIsDrawing;

        private Vector3 topOfPencilPosBefore = Vector3.zero;

        private LineController currentLine;

        public bool SyncOwnerIsDrawing
        {
            get => syncOwnerIsDrawing;
        }

        private void Update()
        {
            if (pencilModeController.SyncIsEraserMode) return;

            if (!Networking.IsOwner(gameObject)) return;

            if (!syncOwnerIsDrawing) return;

            if (!GetIsModerateLength()) return;

            if (currentLine == null) GenerateLine();

            if (currentLine.GetIsMaxLinePositions())
            {
                GenerateLine();

                if (linesParentTran.childCount >= 2)
                {
                    currentLine.AddNewLinePosition(linesParentTran.GetChild(linesParentTran.childCount - 2).GetComponent<LineController>().GetLastLinePosition());

                    currentLine.AddNewLinePosition(topOfPencilTran.position);

                    GenerateLine();
                }
            }

            currentLine.AddNewLinePosition(topOfPencilTran.position);
        }

        private bool GetIsModerateLength()
        {
            if (topOfPencilPosBefore == Vector3.zero)
            {
                topOfPencilPosBefore = topOfPencilTran.position;

                return true;
            }

            if ((topOfPencilTran.position - topOfPencilPosBefore).magnitude >= lengthBetweenLinePos)
            {
                topOfPencilPosBefore = topOfPencilTran.position;

                return true;
            }

            return false;
        }

        public override void OnDeserialization()
        {
            if (Networking.IsOwner(gameObject)) return;

            DeleteAllLines();

            RestoreLines();
        }

        private void RestoreLines()
        {
            if (currentLine == null) GenerateLine();

            for (int i = 0; i < syncLinePositions.Length; i++)
            {
                if ((i + 1) % currentLine.MaxLinePositionsCount == 1) GenerateLine();

                currentLine.AddNewLinePosition(syncLinePositions[i]);
            }
        }

        public void OnPickupAndUseDown()
        {
            if (pencilModeController.SyncIsEraserMode) return;

            Networking.SetOwner(Networking.LocalPlayer, gameObject);

            GenerateLine();

            syncOwnerIsDrawing = true;

            RequestSerialization();
        }

        private void GenerateLine()
        {
            if (currentLine != null)
            {
                while (currentLine.GetLinePositionCount() < currentLine.MaxLinePositionsCount)
                {
                    currentLine.AddNewLinePosition(currentLine.GetLastLinePosition());
                }
            }

            currentLine = VRCInstantiate(objPencilLinePrefab.gameObject).GetComponent<LineController>();

            currentLine.gameObject.transform.SetParent(linesParentTran);

            currentLine.SetUpLine(this, pencilModeController, topOfPencilTran, pencilColor);
        }

        public void OnPickupAndUseUp()
        {
            if (pencilModeController.SyncIsEraserMode) return;

            if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);

            syncOwnerIsDrawing = false;

            RequestSerialization();
        }

        public void AddSyncLinePositions(Vector3 addValue)
        {
            if (!Networking.IsOwner(gameObject)) return;

            Vector3[] syncLinePositionsBefore = new Vector3[syncLinePositions.Length];

            Array.Copy(syncLinePositions, syncLinePositionsBefore, syncLinePositions.Length);

            syncLinePositions = new Vector3[syncLinePositions.Length + 1];

            for (int i = 0; i < syncLinePositions.Length; i++)
            {
                if (i == syncLinePositions.Length - 1)
                {
                    syncLinePositions[i] = addValue;

                    break;
                }

                syncLinePositions[i] = syncLinePositionsBefore[i];
            }

            RequestSerialization();
        }

        public void DeleteAllLines()
        {
            for (int i = 0; i < linesParentTran.childCount; i++) { Destroy(linesParentTran.GetChild(i).gameObject); }

            if (!Networking.IsOwner(gameObject)) return;

            syncLinePositions = new Vector3[0];

            RequestSerialization();
        }

        public void ResetSyncLinePositions(LineController touchedEraserLine)
        {
            if (!Networking.IsOwner(gameObject)) return;

            int maxLinePositionsCount = touchedEraserLine.MaxLinePositionsCount;

            LineController[] lines = new LineController[linesParentTran.childCount];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = linesParentTran.GetChild(i).GetComponent<LineController>();
            }

            int touchedEraserLineNum = Array.IndexOf(lines, touchedEraserLine);

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == lines.Length - 1) break;

                if (i < touchedEraserLineNum) continue;

                lines[i] = lines[i + 1];
            }

            syncLinePositions = new Vector3[(lines.Length - 1) * maxLinePositionsCount];

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == lines.Length - 1) break;

                for (int j = 0; j < maxLinePositionsCount; j++)
                {
                    syncLinePositions[(i * maxLinePositionsCount) + j] = lines[i].GetLineRenderer().GetPosition(j);
                }
            }

            RequestSerialization();

            Destroy(touchedEraserLine.gameObject);
        }
    }
}