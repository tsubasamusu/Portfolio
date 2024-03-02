using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace CallOfUnity
{
    public class BulletDetailBase : MonoBehaviour
    {
        private WeaponDataSO.WeaponData weaponData;

        private int myTeamNo;

        private bool isPlayerBullet;

        public WeaponDataSO.WeaponData WeaponData { get => weaponData; }

        public int MyTeamNo { get => myTeamNo; }

        public bool IsPlayerBullet { get => isPlayerBullet; }

        public void SetUpBullet(WeaponDataSO.WeaponData weaponData, int myTeamNo, bool isPlayerBullet)
        {
            this.weaponData = weaponData;

            this.myTeamNo = myTeamNo;

            this.isPlayerBullet = isPlayerBullet;

            SetUp();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (transform.position.y < 0f || transform.position.y > 5f)
                    {
                        DestroyBullet(false);

                        return;
                    }

                    if (Mathf.Abs((transform.position - Vector3.zero).magnitude) > ConstData.MAX_LENGTH_FROM_CENTER)
                    {
                        DestroyBullet(false);
                    }
                })
                .AddTo(this);

            this.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    if (collision.transform.TryGetComponent(out ControllerBase controllerBase))
                    {
                        Transform bloodEffectTran = Instantiate(GameData.instance.ObjBleedingEffect.transform);

                        bloodEffectTran.position = transform.position;

                        bloodEffectTran.forward = -transform.forward;

                        bloodEffectTran.SetParent(controllerBase.transform);

                        Destroy(bloodEffectTran.gameObject, 0.2f);

                        DestroyBullet(controllerBase.myTeamNo != myTeamNo);
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(SoundManager.instance.GetAudioClip(SoundDataSO.SoundName.銃弾が静的なオブジェクトに当たった時の音), transform.position);

                        Transform impactEffectTran = Instantiate(GameData.instance.ObjImpactBulletEffect.transform);

                        impactEffectTran.position = transform.position;

                        impactEffectTran.forward = -transform.forward;

                        impactEffectTran.SetParent(GameData.instance.TemporaryObjectContainerTran);

                        Destroy(impactEffectTran.gameObject, 0.2f);

                        DestroyBullet(false);
                    }
                })
                .AddTo(this);

            void DestroyBullet(bool attackedEnemy)
            {
                if (!attackedEnemy)
                {
                    Destroy(gameObject);
                }
                else if (isPlayerBullet && attackedEnemy)
                {
                    GameData.instance.playerTotalAttackCount++;
                }
            }
        }

        protected virtual void SetUp()
        {

        }
    }
}