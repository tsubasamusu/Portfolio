namespace yamap
{
    public class HandWeaponDetailBase : WeaponBase
    {
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SeName soundType, float duration)
        {
            this.attackPower = attackPower;

            BulletOwnerType = bulletOwnerType;

            TriggerWeapon();

            if (soundType != SeName.None)
            {
                SoundManager.instance.PlaySE(soundType);
            }

            Destroy(gameObject, duration);
        }

        protected virtual void TriggerWeapon()
        {

        }
    }
}