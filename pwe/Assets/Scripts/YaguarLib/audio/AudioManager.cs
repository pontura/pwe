using UnityEngine;
using System;
using UnityEngine.Audio;

namespace YaguarLib.Audio
{
    public class AudioManager : MonoBehaviour
    {
        static AudioManager mInstance = null;

        public enum channels
        {
            UI,
            GAME,
            MUSIC,
            VOICES
        }
        public enum types
        {
            UI_GENERIC,
            REWARD,
            POPUP,
            CANCEL,
            NONE,
            DRAG,
            DONE,
            RELEASE,
            RESPAWN,
            SNAP,
            COOK,
            FINISH,
            TAP,
            COOKING_MUSIC,
            INGREDIENT_OTHER,
            GAME_DONE
        }
       

        float masterVol, musicVol, sfxVol;
        [SerializeField] AudioMixerGroup masterGroup;
        [SerializeField] AudioMixerGroup musicGroup;
        [SerializeField] AudioMixerGroup sfxGroup;

        public AudioData[] audios;

        public bool Mute { get; private set; }

        [Serializable]
        public class AudioSourceManager
        {
            public channels channel;
            public AudioSource audioSource;
        }
        [Serializable]
        public class AudioData
        {
            public types TYPE;
            public AudioClip clip;
        }
        public AudioClip[] generics;
        public AudioSourceManager[] all;

        public AudioClip GetGeneric(string name)
        {
            print("GetGeneric");
            foreach (AudioClip c in generics)
            {
                print("AudioClip " + c.name.ToString().ToLower()  + " :: " +  name.ToString().ToLower());
                if (c.name.ToString().ToLower() == name.ToString().ToLower()) return c;
            }
            return null;
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
            if (muteValue == 1) Mute = true;

            DontDestroyOnLoad(this.gameObject);
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
            YaguarLib.Events.Events.StopAllSounds += StopAllSounds;
            YaguarLib.Events.Events.StopChannel += StopChannel;
            YaguarLib.Events.Events.PlayGenericSound += PlayGenericSound;
        }
        void OnDestroy()
        {
            YaguarLib.Events.Events.OnPlaySound -= OnPlaySound;
            YaguarLib.Events.Events.OnPlaySoundInChannel -= OnPlaySoundInChannel;
            YaguarLib.Events.Events.StopAllSounds -= StopAllSounds;
            YaguarLib.Events.Events.StopChannel -= StopChannel;
            YaguarLib.Events.Events.PlayGenericSound -= PlayGenericSound;
        }

        private void PlayGenericSound(AudioClip audioClip, channels channel)
        {
            PlaySound(audioClip, channel);
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
        public void StopAudioSource(channels channel)
        {
            AudioSource audioSource = GetAudioSource(channel);            
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
        public void StopChannel(channels channel)
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.channel == channel)
                {
                    m.audioSource.Stop();
                    return;
                }
            }
        }
        void OnPlaySound(types type)
        {
            OnPlaySoundInChannel(type, channels.GAME);
        }
        void OnPlaySoundInChannel(types type, channels channel)
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
        public void ChangePitch(channels channel, float pitch)
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.channel == channel)
                    m.audioSource.pitch = pitch;
            }
        }
        public void ChangeVolume(channels channel, float volume)
        {
            if (!CanPlay()) return;
            foreach (AudioSourceManager m in all)
            {
                if (m.channel == channel)
                    m.audioSource.volume = volume;
            }
        }
        public void PlaySpecificSoundInArray(AudioClip[] allClips)
        {
            PlaySound(allClips[UnityEngine.Random.Range(0, allClips.Length)]);
        }

        public void PlaySound(AudioClip audioClip, channels channel = channels.GAME, float volume = 1f, bool loop = false, bool noRepeat = false)
        {
            if (!CanPlay()) return;
            AudioSource audioSource = GetAudioSource(channel); if (audioSource == null) return;
            if (noRepeat)
            {
                if (audioSource.clip == audioClip && audioSource.isPlaying)
                    return;
            }
            PlaySound(audioSource, audioClip, volume, loop);
        }

        public void PlaySoundOneShot(channels channel, string audioName, bool noRepeat = false)
        {
            AudioSource audioSource = GetAudioSource(channel);
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

            print("PlaySoundPlaySound " + source.name + " " + clip.name + " loop " + loop);
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

        AudioSource GetAudioSource(channels channel)
        {
            foreach (AudioSourceManager m in all)
            {
                if (m.channel == channel)
                    return m.audioSource;
            }
            return null;
        }
    }
}