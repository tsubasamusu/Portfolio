using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CallOfUnity
{
    public class NpcGenerator : MonoBehaviour, ISetUp
    {
        public void SetUp()
        {
            List<Vector3> spawnPosList = GetSpawnPosList();

            for (int i = 0; i < spawnPosList.Count; i++)
            {
                ControllerBase npcControllerBase = Instantiate(GameData.instance.NpcControllerBase);

                npcControllerBase.transform.position = spawnPosList[i];

                npcControllerBase.myTeamNo = i <= ConstData.TEAMMATE_NUMBER - 2 ? 0 : 1;

                if (npcControllerBase.myTeamNo == 1) npcControllerBase.transform.Rotate(0, 180f, 0);

                npcControllerBase.SetUp();

                GameData.instance.npcControllerBaseList.Add(npcControllerBase);
            }
        }

        private List<Vector3> GetSpawnPosList()
        {
            List<Vector3> spawnPosList = new();

            for (int i = 0; i < ConstData.TEAMMATE_NUMBER * 2 - 1; i++)
            {
                if (i <= ConstData.TEAMMATE_NUMBER - 2)
                {
                    float firstPosX = -2f * ((ConstData.TEAMMATE_NUMBER - 1) / 2f);

                    spawnPosList.Add(new Vector3(firstPosX + (2f * i), 0f, -25f));

                    continue;
                }

                float firstPosX2 = -2f * (ConstData.TEAMMATE_NUMBER / 2f);

                spawnPosList.Add(new Vector3(firstPosX2 + (2f * (i - (ConstData.TEAMMATE_NUMBER - 1))), 0f, 25f));
            }

            return spawnPosList;
        }
    }
}