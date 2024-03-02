using UnityEngine;

namespace yamap
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [SerializeField]
        private SoundDataSO soundDataSO;

        private AudioSource[] seSources;

        [SerializeField]
        private AudioSource bgmSource;

        private int seAudioCount = 16;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            seSources = new AudioSource[seAudioCount];

            for (int i = 0; i < seAudioCount; i++)
            {
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlaySE(SeName soundEffectName, bool isLoop = false, float volume = 1f)
        {
            SeData seData = GetSoundEffectData(soundEffectName);

            foreach (var audioSource in seSources)
            {
                if (audioSource.isPlaying)
                {
                    continue;
                }
                else
                {
                    audioSource.loop = isLoop;

                    audioSource.volume = volume;

                    audioSource.PlayOneShot(seData.audioClip);

                    break;
                }
            }

            SeData GetSoundEffectData(SeName searchSeName)
            {
                return soundDataSO.seList.Find(x => x.seName == searchSeName);
            }
        }

        public void ClearAudioSource()
        {
            for (int i = 0; i < seSources.Length; i++)
            {
                if (seSources[i].isPlaying)
                {
                    seSources[i].clip = null;
                }
            }
        }

        public void PlayBGM(BgmName bgmName, bool isLoop, float volume = 1f)
        {
            BgmData bgmData = GetBgmData(bgmName);

            BgmData GetBgmData(BgmName searchbgmName)
            {
                return soundDataSO.bgmList.Find(x => x.bgmName == searchbgmName);
            }

            bgmSource.loop = isLoop;

            bgmSource.volume = volume;

            bgmSource.clip = bgmData.audioClip;

            bgmSource.Play();
        }
    }
}