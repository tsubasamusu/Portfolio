using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum BgmName
    {
        Main,
        Game
    }

    [Serializable]
    public class BgmData
    {
        public BgmName bgmName;

        public AudioClip clip;
    }

    public List<BgmData> bgmDataList = new List<BgmData>();

    public enum SoundEffectName
    {
        Select,
        Cliff,
        jump,
        Explosion,
        Dead,
    }

    [Serializable]
    public class SoundEffectData
    {
        public SoundEffectName soundEffectName;

        public AudioClip clip;
    }

    public List<SoundEffectData> soundEffectDataList = new List<SoundEffectData>();

    public enum VoiceName
    {
        MashiroName,
        TamakoName,
        CountDown,
        GameSet,
    }

    [Serializable]
    public class VoiceData
    {
        public VoiceName voiceName;

        public AudioClip clip;
    }

    public List<VoiceData> voiceDataList = new List<VoiceData>();

    [Serializable]
    public class CharacterVoiceData
    {
        public CharacterManager.CharaName charaName;

        public AudioClip clip;
    }

    public List<CharacterVoiceData> characterVoiceDataList = new();
}