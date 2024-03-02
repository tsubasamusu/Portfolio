using UnityEngine;

namespace yamap
{
    public enum BulletOwnerType
    {
        Player,
        Enemy
    }

    public class WeaponBase : MonoBehaviour
    {
        protected float attackPower;

        protected Rigidbody rb;

        protected BulletOwnerType bulletOwnerType;

        public BulletOwnerType BulletOwnerType
        {
            get => bulletOwnerType;

            set => bulletOwnerType = value;
        }

        protected void Reset()
        {
            if (!TryGetComponent(out rb))
            {
                Debug.Log("Rigidbody ‚ªæ“¾o—ˆ‚Ü‚¹‚ñB");
            }
        }
    }
}