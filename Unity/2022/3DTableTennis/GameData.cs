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

    [SerializeField, Header("���P�b�g���\����̂ɗv���鎞��")]
    private float prepareRacketTime;

    [SerializeField, Header("���P�b�g��U�鎞��")]
    private float swingTime;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�d��")]
    private float gravity;

    [SerializeField, Header("�{�[���̑���")]
    private float ballSpeed;

    [SerializeField, Header("�G�l�~�[�̍U������")]
    private float enemyShotRange;

    [SerializeField, Header("�G�l�~�[���T�[�u��ł܂ł̎���")]
    private float enemyServeTime;

    [SerializeField, Header("���_��\�����鎞��")]
    private float displayScoreTime;

    [SerializeField, Header("���̃t�F�[�h�A�E�g����")]
    private float fadeOutTime;

    [SerializeField, Header("�ō����_")]
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