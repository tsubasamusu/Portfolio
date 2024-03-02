using UnityEngine;

namespace yamap
{
    public class GameData : MonoBehaviour
    {
        public static GameData instance;

        [SerializeField]
        private float fallSpeed;

        public float FallSpeed
        {
            get
            {
                return fallSpeed;
            }
        }

        private int killCount;

        public int KillCount
        {
            get
            {
                return killCount;
            }
            set
            {
                killCount = value;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}