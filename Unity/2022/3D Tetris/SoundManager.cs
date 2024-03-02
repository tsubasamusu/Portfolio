using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private SoundDataSO soundDataSO;

    [SerializeField]
    private AudioSource mainAud;

    [SerializeField]
    private AudioSource subAud;

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

    public void PlaySound(SoundDataSO.SoundName name, bool loop = false)
    {
        AudioClip clip = soundDataSO.soundDataList.Find(x => x.name == name).clip;

        if (loop)
        {
            mainAud.clip = clip;

            mainAud.loop = loop;

            mainAud.Play();
        }
        else
        {
            subAud.PlayOneShot(clip);
        }
    }

    public void StopSound(float fadeOutTime = 0f)
    {
        mainAud.DOFade(0f, fadeOutTime);
    }
}