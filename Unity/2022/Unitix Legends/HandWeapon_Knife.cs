using DG.Tweening;

namespace yamap
{
    public class HandWeapon_Knife : HandWeaponDetailBase
    {
        protected override void TriggerWeapon()
        {
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}