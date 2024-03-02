using System.Collections.Generic;
using System;
using UnityEngine;

namespace CallOfUnity
{
    [CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
    public class SoundDataSO : ScriptableObject
    {
        public enum SoundName
        {
            ゲームスタートボタンを押した時の音,
            普通のボタンを押した時の音,
            無効なボタンを押した時の音,
            ゲームクリアの音,
            ゲームオーバーの音,
            被弾した時の音,
            爆発した時の音,
            リロードする時の音,
            試合中のBGM,
            銃弾が静的なオブジェクトに当たった時の音
        }

        [Serializable]
        public class SoundData
        {
            public SoundName name;

            public AudioClip clip;
        }

        public List<SoundData> soundDataList = new();
    }
}