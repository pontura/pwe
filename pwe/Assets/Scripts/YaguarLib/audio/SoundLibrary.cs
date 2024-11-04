using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaguarLib.Audio
{

    [CreateAssetMenu(fileName = "SoundLibrary", menuName = "YaguarLib/SoundLibrary")]
    public class SoundLibrary : ScriptableObject {
        [Header("Audio Clips")]
        public List<ClipData> audioClips;

        [Header("Audio Clip Groups")]
        public List<ClipGroupData> audioClipGroups;

        public ClipData GetClip(string key)
        {
            return audioClips.Find(x => x.key == key);
        }

        public ClipData GetRandomClipFromGroup(string key) {
            return audioClips.Find(x => x.key == key);
        }
    }
}