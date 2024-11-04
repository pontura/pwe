using UnityEngine;
using System;
using UnityEngine.Audio;

namespace YaguarLib.Audio
{
    public class AudioManager : MonoBehaviour
    {
        static AudioManager mInstance = null;

        public types TYPE;
        public enum types
        {
            UI_GENERIC,
            REWARD,
            POPUP,
            CANCEL,
            NONE
        }

        float masterVol, musicVol, sfxVol;
        [SerializeField] AudioMixerGroup masterGroup;
        [SerializeField] AudioMixerGroup musicGroup;
        [SerializeField] AudioMixerGroup sfxGroup;

        public AudioData[] audios;
        public AudioSourceManager[] all;

        public bool Mute { get; private set; }

        [Serializable]
        public class AudioSourceManager
        {
            public string sourceName;
            public AudioSource audioSource;
        }
        [Serializable]
        public class AudioData
        {
            public types TYPE;
            public AudioClip clip;
        }

        public static AudioManager Instance
        {
            get
            {
                return mInstance;
            }
        }
        void Awake()
        {
            if (!mInstance)
                mInstance = this;

            if (!masterGroup.audioMixer.GetFloat("masterVol", out masterVol))
                masterVol = 0f;

            if (!musicGroup.audioMixer.GetFloat("musicVol", out musicVol))
                musicVol = 0f;

            if (!sfxGroup.audioMixer.GetFloat("sfxVol", out sfxVol))
                sfxVol = 0f;

            int muteValue = PlayerPrefs.GetInt("mute", 0);
            if (muteValue == 1) Mute = true;
            DontDestroyOnLoad(this);
        }
        void Start()
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.audioSource == null)
                    m.audioSource = gameObject.AddComponent<AudioSource>();
            }
            YaguarLib.Events.Events.OnPlaySound += OnPlaySound;
            YaguarLib.Events.Events.OnPlaySoundInChannel += OnPlaySoundInChannel;
        }
        void OnDestroy()
        {
            YaguarLib.Events.Events.OnPlaySound -= OnPlaySound;
            YaguarLib.Events.Events.OnPlaySoundInChannel -= OnPlaySoundInChannel;
        }

        public void MusicEnable(bool enable) {
            Debug.Log("MusicEnable " + enable);
            float val = enable ? musicVol : -80f;
            musicGroup.audioMixer.SetFloat("musicVol", val);
        }
        public void SoundEnable(bool enable) {
            Debug.Log("SoundEnable " + enable);
            PlayerPrefs.SetInt("mute", enable ? 0 : 1);
            Mute = !enable;
            float val = enable ? masterVol : -80f;
            masterGroup.audioMixer.SetFloat("masterVol", val);
        }

        public void SfxEnable(bool enable) {
            Debug.Log("SfxEnable " + enable);
            float val = enable ? sfxVol : -80f;
            sfxGroup.audioMixer.SetFloat("sfxVol", val);
        }

        public bool CanPlay()
        {
            if (Mute) return false;
            return true;
        }
        public void StopAudioSource(string audioSourceName)
        {
            AudioSource audioSource = GetAudioSource(audioSourceName);            
            if (audioSource != null)
                audioSource.Stop();
        }
        public void StopAllSounds()
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.audioSource != null)
                    m.audioSource.Stop();
            }
        }
        void OnPlaySound(types type)
        {
            string audioSource = "ui";
            if (type == types.REWARD)
                audioSource = "ui2";
            OnPlaySoundInChannel(type, audioSource);
        }
        void OnPlaySoundInChannel(types type, string channel)
        {
            AudioData ad = GetAudio(type);
            if (ad == null) return;
            PlaySound(ad.clip, channel);
        }
        AudioData GetAudio(types t)
        {
            foreach (AudioData ad in audios)
                if (t == ad.TYPE) return ad;
            return null;
        }
        public void ChangePitch(string sourceName, float pitch)
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.sourceName == sourceName)
                    m.audioSource.pitch = pitch;
            }
        }
        public void ChangeVolume(string sourceName, float volume)
        {
            if (!CanPlay()) return;
            foreach (AudioSourceManager m in all)
            {
                if (m.sourceName == sourceName)
                    m.audioSource.volume = volume;
            }
        }
        public void PlaySpecificSoundInArray(AudioClip[] allClips)
        {
            PlaySound(allClips[UnityEngine.Random.Range(0, allClips.Length)]);
        }

        public void PlaySound(AudioClip audioClip, string sourceName = "common", float volume = 1f, bool loop = false, bool noRepeat = false)
        {
            if (!CanPlay()) return;
            AudioSource audioSource = GetAudioSource(sourceName); if (audioSource == null) return;
            if (noRepeat)
            {
                if (audioSource.clip == audioClip && audioSource.isPlaying)
                    return;
            }
            PlaySound(audioSource, audioClip, volume, loop);
        }

        public void PlaySoundOneShot(string sourceName, string audioName, bool noRepeat = false)
        {
            AudioSource audioSource = GetAudioSource(sourceName);
            if (audioSource == null) return;

            if (audioName == "")
            {
                audioSource.Stop(); return;
            }

            if (!CanPlay()) return;

            AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioName) as AudioClip;
            if (noRepeat)
            {
                if (audioSource.clip == clip && audioSource.isPlaying)
                    return;
            }
            audioSource.PlayOneShot(clip);
        }

        public void PlaySound(AudioSource source, AudioClip clip, float volume = 1, bool loop = false) {
            source.volume = volume;
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }

        public void PlaySoundOneShot(AudioSource source, ClipData clip) {
            PlaySoundOneShot(source, clip.clip, clip.vol);
        }

        public void PlaySoundOneShot(AudioSource source, AudioClip clip, float volume = 1) {
            source.volume = volume;
            source.PlayOneShot(clip);
        }

        AudioSource GetAudioSource(string sourceName)
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.sourceName == sourceName)
                    return m.audioSource;
            }
            return null;
        }
    }
}