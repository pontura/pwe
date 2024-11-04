using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace YaguarLib.Audio
{
    public class IngameAudioManager : MonoBehaviour
    {
        [SerializeField] SoundLibrary soundLibrary;
        [SerializeField] AudioSource source;
        
        public void Play(string key) {
            ClipData cp = soundLibrary.GetClip(key);
            if (cp == null)
                return;
            AudioManager.Instance.PlaySound(source, cp.clip, cp.vol);
        }

        public void PlayOneShot(string key) {
            ClipData cp = soundLibrary.GetClip(key);
            if (cp == null)
                return;
            AudioManager.Instance.PlaySoundOneShot(source, cp.clip, cp.vol);
        }

        public void Play(string key, string sourceKey) {
            ClipData cp = soundLibrary.GetClip(key);
            if (cp == null)
                return;
            AudioManager.Instance.PlaySound(cp.clip, sourceName:sourceKey, volume:cp.vol);
        }

    }
}
