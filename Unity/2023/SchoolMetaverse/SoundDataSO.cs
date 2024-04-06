using System.Collections.Generic; 
using System; 
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum SoundName
    {
        ボタンを押した時の音,
        無効なボタンを押した時の音,
        歩く時の足音,
        BGM,
        送信ボタンを押した時の音,
        エラーを表示する時の音
    }

    [Serializable]
    public class SoundData
    {
        public SoundName name;

        public AudioClip clip;
    }

    public List<SoundData> soundDataList = new();
}