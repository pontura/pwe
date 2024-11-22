using System;
using UnityEngine;
using System.Collections.Generic;
using Pwe.Games.SolarSystem;

namespace Pwe.Games.Compare
{
    [CreateAssetMenu(fileName = "CompareLevels", menuName = "Compare/Data/Levels", order = 0)]
    public class LevelsData : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> levels { get; private set; }

        [Serializable]
        public class LevelData
        {
            public List<ItemData> ingredients;
            public int ovenDuration;
        }
    }
}
