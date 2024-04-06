using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace CallOfUnity
{
    public class CharacterHealth : MonoBehaviour, ISetUp
    {
        public void SetUp()
        {
            float hp = 100f;

            Rigidbody rb = GetComponent<Rigidbody>();

            ControllerBase controllerBase = GetComponent<ControllerBase>();

            this.OnCollisionEnterAsObservable()
                .Where(collision => collision.transform.TryGetComponent(out BulletDetailBase _))
                .Subscribe(collision =>
                {
                    if (!rb.isKinematic) rb.isKinematic = true;

                    BulletDetailBase bulletDetailBase = collision.transform.GetComponent<BulletDetailBase>();

                    if (bulletDetailBase.MyTeamNo != controllerBase.myTeamNo)
                    {
                        hp = Mathf.Clamp(hp - bulletDetailBase.WeaponData.attackPower, 0f, 100f);

                        if (controllerBase.IsPlayer)
                        {
                            GameData.instance.UiManager.SetSldHp(hp / 100f);

                            SoundManager.instance.PlaySound(SoundDataSO.SoundName.”í’e‚µ‚½Žž‚Ì‰¹);
                        }
                        else if (bulletDetailBase.IsPlayerBullet)
                        {
                            GameData.instance.UiManager.PlayTxtGaveDamageAnimation(bulletDetailBase.WeaponData.attackPower);
                        }
                    }

                    Destroy(collision.gameObject);

                    if (hp == 0f) Die(ref hp, controllerBase, bulletDetailBase);
                })
                .AddTo(this);
        }

        public void Die(ref float hp, ControllerBase controllerBase, BulletDetailBase bulletDetailBase = null)
        {
            if (controllerBase.IsPlayer)
            {
                GameData.instance.playerTotalDeathCount++;

                GameData.instance.UiManager.StopReloadGaugeAnimation();
            }

            if (bulletDetailBase != null && bulletDetailBase.IsPlayerBullet)
            {
                GameData.instance.playerTotalKillCount++;
            }

            if (controllerBase.myTeamNo == 0)
            {
                GameData.instance.Score.Value = (GameData.instance.Score.Value.team0, (GameData.instance.Score.Value.team1 + 1));
            }
            else
            {
                GameData.instance.Score.Value = ((GameData.instance.Score.Value.team0 + 1), GameData.instance.Score.Value.team1);
            }

            controllerBase.ReSetUp();

            hp = 100f;

            if (controllerBase.IsPlayer) GameData.instance.UiManager.SetSldHp(1f);

            GameData.instance.UiManager.UpdateTxtScore();

            transform.position = controllerBase.myTeamNo == 0 ? GameData.instance.RespawnTransList[0].position : GameData.instance.RespawnTransList[1].position;
        }
    }
}