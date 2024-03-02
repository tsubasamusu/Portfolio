using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public enum SoundName
    {
        GameStartSE,
        GameClearSE,
        RestartSE,
        MainBGM,
    }

    [Serializable]
    public class SoundData
    {
        public SoundName name;

        public AudioClip clip;
    }

    [SerializeField]
    private List<SoundData> soundDatasList = new List<SoundData>();

    [SerializeField]
    private AudioSource mainAud;

    [SerializeField]
    private AudioSource subAud;

    public static SoundManager instance;

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

    public AudioClip GetAudioClip(SoundName name)
    {
        return soundDatasList.Find(x => x.name == name).clip;
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        if (loop)
        {
            mainAud.loop = true;

            mainAud.clip = clip;

            mainAud.Play();
        }
        else
        {
            subAud.PlayOneShot(clip);
        }
    }

    public void StopMainSound(float fadeTime = 0f)
    {
        mainAud.DOFade(0f, fadeTime);
    }
}