using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField, Header("ラケットを構えるのに要する時間")]
    private float prepareRacketTime;

    [SerializeField, Header("ラケットを振る時間")]
    private float swingTime;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("重力")]
    private float gravity;

    [SerializeField, Header("ボールの速さ")]
    private float ballSpeed;

    [SerializeField, Header("エネミーの攻撃圏内")]
    private float enemyShotRange;

    [SerializeField, Header("エネミーがサーブを打つまでの時間")]
    private float enemyServeTime;

    [SerializeField, Header("得点を表示する時間")]
    private float displayScoreTime;

    [SerializeField, Header("音のフェードアウト時間")]
    private float fadeOutTime;

    [SerializeField, Header("最高得点")]
    private int maxScore;

    [HideInInspector]
    public (int playerScore, int enemyScore) score;

    public float PrepareRacketTime { get => prepareRacketTime; }

    public float SwingTime { get => swingTime; }

    public float MoveSpeed { get => moveSpeed; }

    public float Gravity { get => gravity; }

    public float BallSpeed { get => ballSpeed; }

    public float EnemyShotRange { get => enemyShotRange; }

    public float EnemyServeTime { get => enemyServeTime; }

    public float DisplayScoreTime { get => displayScoreTime; }

    public float FadeOutTime { get => fadeOutTime; }

    public int MaxScore { get => maxScore; }
}