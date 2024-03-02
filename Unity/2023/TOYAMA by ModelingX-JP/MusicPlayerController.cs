using UdonSharp;
using UnityEngine;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class MusicPlayerController : UdonSharpBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip bgmClip;

    [SerializeField, Range(0f, 1f)]
    private float bgmVolume = 0.5f;

    public void Start()
    {
        audioSource.volume = bgmVolume;

        audioSource.clip = bgmClip;

        audioSource.Play();
    }
}