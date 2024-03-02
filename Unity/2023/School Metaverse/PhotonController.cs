using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SchoolMetaverse
{
    public class PhotonController : MonoBehaviourPunCallbacks
    {
        private bool isConnecting;

        private bool joinedRoom;

        public bool JoinedRoom
        {
            get => joinedRoom;
        }

        public void ConnectMasterServer()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                RoomOptions roomOptions = new();

                roomOptions.MaxPlayers = ConstData.MAX_PLAYERS;

                PhotonNetwork.JoinOrCreateRoom("room", roomOptions, TypedLobby.Default);

                isConnecting = false;
            }
        }

        public override void OnJoinedRoom()
        {
            joinedRoom = true;
        }
    }
}