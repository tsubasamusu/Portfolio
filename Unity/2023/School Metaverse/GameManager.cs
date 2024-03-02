using Photon.Pun;
using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace SchoolMetaverse
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Transform spawnTran;

        [SerializeField]
        private List<SerializableInterface<ISetUp>> setUpList = new();

        private void Start()
        {
            GameObject objPlayer = PhotonNetwork.Instantiate("Player", spawnTran.position, Quaternion.identity);

            objPlayer.GetComponent<PlayerController>().SetUp();

            Camera.main.transform.SetParent(objPlayer.transform);

            Camera.main.transform.localPosition = new(0f, ConstData.CAMERA_HEIGHT, 0f);

            SoundManager.instance.PlaySound(SoundDataSO.SoundName.BGM, true, GameData.instance.bgmVolume);

            for (int i = 0; i < setUpList.Count; i++)
            {
                setUpList[i].Value.SetUp();
            }
        }
    }
}