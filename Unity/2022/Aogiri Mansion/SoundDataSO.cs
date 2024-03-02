using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum SoundEffectName
    {
        None,
        GameStartSE,
        NormalHorrorSE,
        LoomingSE,
        NoriNoriSE,
        ReroReroSE,
    }

    [Serializable]
    public class SoundEffectData
    {
        public SoundEffectName name;
        public AudioClip clip;
    }

    public List<SoundEffectData> soundEffectDataList = new List<SoundEffectData>();
}