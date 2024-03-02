using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using UnityEngine;

namespace SchoolMetaverse
{
    [RequireComponent(typeof(PhotonView))]
    public class MessageManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private UIManagerMain uiManagerMain;

        public void PrepareSendMessage(string senderName, string message)
        {
            photonView.RPC(nameof(SendMessage), RpcTarget.All, senderName, message);
        }

        [PunRPC]
        private void SendMessage(string senderName, string message)
        {
            for (int i = 0; i < GameData.instance.messages.Length; i++)
            {
                if (i == 0) continue;

                GameData.instance.messages[i - 1] = GameData.instance.messages[i];
            }

            GameData.instance.messages[ConstData.MAX_MESSAGE_LINES - 1] = "　" + senderName + "：" + message + "\n";

            uiManagerMain.UpdateTxtMessage();

            var hashtable = new ExitGames.Client.Photon.Hashtable
            {
                ["SendMessageCount"] = PhotonNetwork.CurrentRoom.CustomProperties["SendMessageCount"] is int count ? count + 1 : 1
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

            SendMessageFromBotAsync(this.GetCancellationTokenOnDestroy(), newPlayer, "さんが参加しました。").Forget();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

            SendMessageFromBotAsync(this.GetCancellationTokenOnDestroy(), otherPlayer, "さんが退出しました。").Forget();
        }

        private async UniTaskVoid SendMessageFromBotAsync(CancellationToken token, Player player, string message)
        {
            await UniTask.WaitUntil(() => player.NickName != string.Empty, cancellationToken: token);

            PrepareSendMessage("Bot", player.NickName + message);
        }
    }
}