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
            �Q�[���X�^�[�g�{�^�������������̉�,
            ���ʂ̃{�^�������������̉�,
            �����ȃ{�^�������������̉�,
            �Q�[���N���A�̉�,
            �Q�[���I�[�o�[�̉�,
            ��e�������̉�,
            �����������̉�,
            �����[�h���鎞�̉�,
            ��������BGM,
            �e�e���ÓI�ȃI�u�W�F�N�g�ɓ����������̉�
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