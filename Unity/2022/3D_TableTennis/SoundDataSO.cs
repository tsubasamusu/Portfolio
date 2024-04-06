using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum SoundName
    {
        MainBGM,
        GameStartSE,
        GameRestartSE,
        GameOverSE,
        PlayerPointSE,
        EnemyPointSE,
        RacketSE,
        BoundSE,
    }

    [Serializable]
    public class SoundData
    {
        public SoundName name;
        public AudioClip clip;
    }

    public List<SoundData> soundDataList = new();
}