using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace CallOfUnity
{
    public class Bullet_RocketLauncher : BulletDetailBase
    {
        protected override void SetUp()
        {
            base.SetUp();

            Transform effectTran = Instantiate(GameData.instance.ObjRocketLauncherEffect.transform);

            effectTran.SetParent(transform);

            effectTran.forward = -transform.forward;

            this.UpdateAsObservable()
                .Subscribe(_ => effectTran.position = transform.position)
                .AddTo(this);

            this.OnCollisionEnterAsObservable()
                .Subscribe(_ =>
                {
                    AudioSource.PlayClipAtPoint(SoundManager.instance.GetAudioClip(SoundDataSO.SoundName.”š”­‚µ‚½Žž‚Ì‰¹), transform.position);

                    Transform explosionEffectTran = Instantiate(GameData.instance.ObjExplosionEffect.transform);

                    explosionEffectTran.position = transform.position;

                    explosionEffectTran.SetParent(GameData.instance.TemporaryObjectContainerTran);

                    Destroy(explosionEffectTran.gameObject, 1f);
                })
                .AddTo(this);
        }
    }
}