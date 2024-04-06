using System.Collections.Generic; 
using System; 
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum SoundName
    {
        �{�^�������������̉�,
        �����ȃ{�^�������������̉�,
        �������̑���,
        BGM,
        ���M�{�^�������������̉�,
        �G���[��\�����鎞�̉�
    }

    [Serializable]
    public class SoundData
    {
        public SoundName name;

        public AudioClip clip;
    }

    public List<SoundData> soundDataList = new();
}