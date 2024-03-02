using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleVideoPlayer
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoAudioManager : UdonSharpBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private Slider sldVolume;

        [SerializeField, Range(0f, 1f)]
        private float volumeSliderDefaultValue = 0f;

        public void Start()
        {
            audioSource.volume = sldVolume.value = volumeSliderDefaultValue;
        }

        public void OnSldVolumeChanged()
        {
            audioSource.volume = sldVolume.value;
        }
    }
}