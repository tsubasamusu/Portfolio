using System.Collections.Generic;
using UnityEngine;
using System;

namespace yamap
{
    public enum BgmName
    {
        Main
    }

    public enum SeName
    {
        None,
        KnifeSE,
        BatSE,
        ExplosionSE,
        GasSE,
        AssaultSE,
        ShotgunSE,
        SniperSE,
        RecoverySE,
        BulletSE,
        AirplaneSE,
        FallSE,
        LandingSE,
        RunningSE,
        BePreparedSE,
        GetItemSE,
        DiscardItemSE,
        SelectItemSE,
        GameStartSE,
        GameOverSE,
        GameClearSE,
        NoneItemSE,
        NextShotOkSE,
        InflictDamageSE,
    }

    [Serializable]
    public class SeData
    {
        public SeName seName;

        public AudioClip audioClip;
    }

    [Serializable]
    public class BgmData
    {
        public BgmName bgmName;

        public AudioClip audioClip;
    }

    [CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO_yamap")]
    public class SoundDataSO : ScriptableObject
    {
        public List<SeData> seList = new();

        public List<BgmData> bgmList = new();
    }
}