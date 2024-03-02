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
                Debug.Log("Rigidbody ���擾�o���܂���B");
            }
        }
    }
}