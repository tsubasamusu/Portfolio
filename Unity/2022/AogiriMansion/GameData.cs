using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private int metMembersCount;

    public int MetMembersCount
    {
        get
        {
            return metMembersCount;
        }
        set
        {
            metMembersCount = value;
        }
    }

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