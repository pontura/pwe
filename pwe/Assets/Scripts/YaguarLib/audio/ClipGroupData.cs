using UnityEngine;
using System;
using System.Collections.Generic;

namespace YaguarLib.Audio
{
    [Serializable]
    public class ClipGroupData
    {
        public string key;
        public List<ClipData> clips;
    }
}
