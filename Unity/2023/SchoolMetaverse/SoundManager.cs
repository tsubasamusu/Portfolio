using Photon.Voice.Unity;
using UnityEngine;

namespace SchoolMetaverse
{
    [RequireComponent(typeof(Recorder))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private Recorder recorder;

        [SerializeField]
        private SoundDataSO soundDataSO;

        private AudioSource audBgmPlayer;

        private AudioSource[] audioSources;

        public static SoundManager instance;

        private void Awake()
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
        }

        private void Start()
        {
            audBgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();

            audioSources = new AudioSource[soundDataSO.soundDataList.Count];

            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public AudioClip GetAudioClip(SoundDataSO.SoundName name)
        {
            return soundDataSO.soundDataList.Find(x => x.name == name).clip;
        }

        public void PlaySound(SoundDataSO.SoundName name, bool isBgm = false, float volume = 1f)
        {
            if (isBgm)
            {
                audBgmPlayer.clip = GetAudioClip(name);

                audBgmPlayer.volume = volume;

                audBgmPlayer.loop = true;

                audBgmPlayer.Play();

                return;
            }

            foreach (AudioSource source in audioSources)
            {
                if (source.isPlaying == false)
                {
                    source.clip = GetAudioClip(name);

                    source.volume = volume;

                    source.Play();

                    break;
                }
            }
        }

        public void UpdateBgmVolume()
        {
            audBgmPlayer.volume = GameData.instance.bgmVolume;
        }

        public void SetRecorderActive(bool isactive)
        {
            recorder.RecordingEnabled = !isactive;
        }
    }
}