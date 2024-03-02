using DG.Tweening;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private SoundDataSO soundDataSO;

    private AudioSource[] audioSources;

    void Awake()
    {
        audioSources = new AudioSource[soundDataSO.soundDataList.Count];

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundDataSO.SoundName name, float volume = 1f, bool loop = false)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying == false)
            {
                source.clip = soundDataSO.soundDataList.Find(x => x.name == name).clip;

                source.volume = volume;

                if (loop)
                {
                    source.loop = true;
                }

                source.Play();

                return;
            }
        }
    }

    public void StopSound()
    {
        foreach (AudioSource source in audioSources)
        {
            source.DOFade(0f, GameData.instance.FadeOutTime)
                .OnComplete(() =>
                {
                    source.Stop();

                    source.clip = null;
                });
        }
    }
}