using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public GameObject attackEffect;

    public GameObject deadEffect;

    public float powerRatio;

    public float damageTime;

    public float moveSpeed;

    public float jumpPower;

    [Tooltip("�R���畜������Ƃ��̃W�����v��")]
    public float jumpHeight;

    [Tooltip("�R�ɂ����݂��Ă����鎞��")]
    public float maxCliffTime;

    public float npcMoveSpeed;

    public float npcJumpPower;

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
}