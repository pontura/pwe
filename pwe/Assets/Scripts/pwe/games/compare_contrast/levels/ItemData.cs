using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pwe.Games.Compare
{
    [Serializable]
    public class ItemData
    {
        public enum Items
        {
            strings,
            colors
        }
        public Items item;
        public int num;
    }
}
