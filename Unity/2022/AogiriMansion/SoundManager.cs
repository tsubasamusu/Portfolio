using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private SoundDataSO soundDataSO;

    [SerializeField]
    private AudioSource myAudioSource;

    [SerializeField]
    private AudioSource playerAudioSource;

    private void Start()
    {
        PlaySoundEffectByAudioSource(GetSoundEffectData(SoundDataSO.SoundEffectName.GameStartSE));
    }

    public SoundDataSO.SoundEffectData GetSoundEffectData(SoundDataSO.SoundEffectName name)
    {
        foreach (SoundDataSO.SoundEffectData soundEffectData in soundDataSO.soundEffectDataList)
        {
            if (soundEffectData.name == name)
            {
                return soundEffectData;
            }
        }

        return null;
    }

    public void PlaySoundEffectByAudioSource(SoundDataSO.SoundEffectData soundEffectData)
    {
        myAudioSource.PlayOneShot(soundEffectData.clip);
    }

    public void SetPlayerAudioSourse(bool isSetting)
    {
        playerAudioSource.enabled = isSetting;
    }
}