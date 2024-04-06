using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SchoolMetaverse
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        public void SetUp()
        {
            if (!photonView.IsMine) return;

            PhotonNetwork.LocalPlayer.NickName = GameData.instance.playerName;

            photonView.RPC(nameof(PrepareDisplayPlayerName), RpcTarget.All, GameData.instance.playerName);

            transform.GetChild(0).gameObject.SetActive(false);

            CharacterController characterController = GetComponent<CharacterController>();

            Animator animator = GetComponent<Animator>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    transform.eulerAngles = new(0f, Camera.main.transform.eulerAngles.y, 0f);

                    Vector3 movement = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    movement = Vector3.Scale(Camera.main.transform.forward * movement.z + Camera.main.transform.right * movement.x, new Vector3(1f, 0f, 1f));

                    characterController.Move(ConstData.MOVE_SPEED * Time.deltaTime * movement);

                    AnimationName animationName = GetPressedKey() switch
                    {
                        ConstData.WALK_F_KEY => AnimationName.isWalking_F,
                        ConstData.WALK_R_KEY => AnimationName.isWalking_R,
                        ConstData.WALK_B_KEY => AnimationName.isWalking_B,
                        ConstData.WALK_L_KEY => AnimationName.isWalking_L,
                        _ => AnimationName.Null,
                    };

                    foreach (AnimationName animName in Enum.GetValues(typeof(AnimationName)))
                    {
                        if (animName == AnimationName.Null) break;

                        animator.SetBool(animName.ToString(), animationName == animName);
                    }
                })
                .AddTo(this);

            KeyCode GetPressedKey()
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(code)) return code;
                }

                return KeyCode.None;
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            photonView.RPC(nameof(PrepareDisplayPlayerName), RpcTarget.All, GameData.instance.playerName);
        }

        [PunRPC]
        private void PrepareDisplayPlayerName(string playerName)
        {
            DisplayPlayerNameAsync(playerName, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DisplayPlayerNameAsync(string playerName, CancellationToken token)
        {
            Text txtPlayerName = null;

            while (txtPlayerName == null)
            {
                txtPlayerName = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>();

                await UniTask.Yield(token);
            }

            txtPlayerName.text = playerName;
        }
    }
}