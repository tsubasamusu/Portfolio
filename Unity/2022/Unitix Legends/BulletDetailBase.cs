using UnityEngine;

namespace yamap
{
    public class BulletDetailBase : WeaponBase
    {
        protected float effectDuration;

        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SeName seName, float duration = 3.0f, GameObject effectPrefab = null)
        {
            Reset();

            this.attackPower = attackPower;

            BulletOwnerType = bulletOwnerType;

            TriggerBullet(direction, duration);

            if (seName != SeName.None)
            {
                SoundManager.instance.PlaySE(seName);
            }

            if (effectPrefab != null)
            {
                GenerateEffect(effectPrefab);
            }
        }

        protected virtual void TriggerBullet(Vector3 direction, float duration)
        {
            rb.AddForce(direction);

            Destroy(gameObject, duration);
        }

        protected virtual void GenerateEffect(GameObject effectPrefab)
        {
            GameObject effect = Instantiate(effectPrefab, transform);

            Destroy(effect, effectDuration);
        }

        public float GetAttackPower()
        {
            return attackPower;
        }

        public virtual void AddTriggerBullet(EnemyController enemyController)
        {

        }
    }
}