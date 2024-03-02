using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private SoundDataSO soundDataSO;

    [SerializeField]
    private AudioSource mainAudioSource;

    [SerializeField]
    private AudioSource subAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SoundDataSO.BgmData GetBgmData(SoundDataSO.BgmName bgmName)
    {
        return soundDataSO.bgmDataList.Find(x => x.bgmName == bgmName);
    }

    public SoundDataSO.SoundEffectData GetSoundEffectData(SoundDataSO.SoundEffectName soundEffectName)
    {
        return soundDataSO.soundEffectDataList.Find(x => x.soundEffectName == soundEffectName);
    }

    public SoundDataSO.VoiceData GetVoiceData(SoundDataSO.VoiceName voiceName)
    {
        return soundDataSO.voiceDataList.Find(x => x.voiceName == voiceName);
    }

    public SoundDataSO.CharacterVoiceData GetCharacterVoiceData(CharacterManager.CharaName charaName)
    {
        return soundDataSO.characterVoiceDataList.Find(x => x.charaName == charaName);
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        if (loop == true)
        {
            mainAudioSource.clip = clip;

            mainAudioSource.loop = loop;

            mainAudioSource.Play();
        }
        else
        {
            subAudioSource.PlayOneShot(clip);
        }
    }

    public void StopSound(float time, bool isMain = true)
    {
        if (isMain)
        {
            mainAudioSource.DOFade(0f, time);
        }
        else
        {
            subAudioSource.DOFade(0f, time);
        }
    }

    public void ChangeBgmMainToGame()
    {
        if (mainAudioSource.clip != soundDataSO.bgmDataList.Find(x => x.bgmName == SoundDataSO.BgmName.Main).clip)
        {
            return;
        }

        mainAudioSource.DOFade(0f, 1f)
            .OnComplete(() =>
            {
                PlaySound(GetBgmData(SoundDataSO.BgmName.Game).clip, true);

                mainAudioSource.DOFade(1f, 1f);
            });
    }
}