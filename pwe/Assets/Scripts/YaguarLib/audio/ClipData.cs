using UnityEngine;
using System;

namespace YaguarLib.Audio
{
    [Serializable]
    public class ClipData
    {
        public string key;
        public AudioClip clip;
        public float vol = 1f;
    }
}
