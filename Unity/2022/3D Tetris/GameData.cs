using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [SerializeField]
    private float normalFallSpeed;

    [SerializeField]
    private float specialFallSpeed;

    [SerializeField]
    private float timeLimit;

    [SerializeField, Header("�u���b�N�̐����\��̐�")]
    private int appointmentsNumber;

    [SerializeField, Header("1�񂠂���̓��_")]
    private int scorePerColumn;

    public float NormalFallSpeed
    {
        get
        {
            return normalFallSpeed;
        }
    }

    public float SpecialFallSpeed
    {
        get
        {
            return specialFallSpeed;
        }
    }

    public float TimeLimit
    {
        get
        {
            return timeLimit;
        }
    }

    public int AppointmentsNumber
    {
        get
        {
            return appointmentsNumber;
        }
    }

    public int ScorePerColumn
    {
        get
        {
            return scorePerColumn;
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