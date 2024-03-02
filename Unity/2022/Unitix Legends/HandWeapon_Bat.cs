using UnityEngine;
using DG.Tweening;

namespace yamap
{
    public class HandWeapon_Bat : HandWeaponDetailBase
    {
        protected override void TriggerWeapon()
        {
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);

            transform.DOLocalRotate(new Vector3(60f, 0f, 0f), 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}