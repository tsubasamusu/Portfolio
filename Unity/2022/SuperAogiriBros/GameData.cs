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

    [Tooltip("ŠR‚©‚ç•œŠˆ‚·‚é‚Æ‚«‚ÌƒWƒƒƒ“ƒv—Í")]
    public float jumpHeight;

    [Tooltip("ŠR‚É‚µ‚ª‚Ý‚Â‚¢‚Ä‚¢‚ç‚ê‚éŽžŠÔ")]
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