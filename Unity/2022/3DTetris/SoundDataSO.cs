using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum SoundName
    {
        BGM,
        BtnGameStartSE,
        BtnRestartSE,
        GameClearSE,
        GameOverSE,
        DigestionSE,
        CannotRotSE,
        TenTimeLimitSE,
    }

    [Serializable]
    public class SoundData
    {
        public SoundName name;
        public AudioClip clip;
    }

    public List<SoundData> soundDataList = new List<SoundData>();
}