using UnityEngine;
using System;

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

        public AudioData[] audios;
        public AudioSourceManager[] all;

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

            int muteValue = PlayerPrefs.GetInt("mute", 0);
            if (muteValue == 1) mute = true;
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
            YaguarLib.Events.Events.Mute += Mute;
            SetMuteValues();
        }
        void OnDestroy()
        {
            YaguarLib.Events.Events.OnPlaySound -= OnPlaySound;
            YaguarLib.Events.Events.OnPlaySoundInChannel -= OnPlaySoundInChannel;
            YaguarLib.Events.Events.Mute -= Mute;
        }
        public bool mute;
        void Mute(bool mute)
        {
            int muteValue = 0;
            if (mute) muteValue = 1;
            PlayerPrefs.SetInt("mute", muteValue);
            this.mute = mute;
            SetMuteValues();
        }
        void SetMuteValues()
        {
            float value = 0;
            if (!mute)
                value = 1;
            foreach (AudioSourceManager s in all)
                s.audioSource.volume = value;           
        }
        public bool CanPlay()
        {
            if (mute) return false;
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
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.Play();
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