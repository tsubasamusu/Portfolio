using UnityEngine;

namespace CallOfUnity
{
    public class SoundManager : MonoBehaviour, ISetUp
    {
        public static SoundManager instance;

        private AudioSource[] audioSources;

        void Awake()
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

        public void SetUp()
        {
            audioSources = new AudioSource[GameData.instance.SoundDataSO.soundDataList.Count];

            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public AudioClip GetAudioClip(SoundDataSO.SoundName name)
        {
            return GameData.instance.SoundDataSO.soundDataList.Find(x => x.name == name).clip;
        }

        public void PlaySound(SoundDataSO.SoundName name, float volume = 1f, bool loop = false)
        {
            foreach (AudioSource source in audioSources)
            {
                if (source.isPlaying == false)
                {
                    source.clip = GetAudioClip(name);

                    source.volume = volume;

                    source.loop = loop;

                    source.Play();

                    break;
                }
            }
        }

        public void StopSound()
        {
            foreach (AudioSource source in audioSources)
            {
                source.Stop();

                source.clip = null;
            }
        }
    }
}