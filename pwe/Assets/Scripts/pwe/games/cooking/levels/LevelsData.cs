using System;
using UnityEngine;
using System.Collections.Generic;
using Pwe.Games.SolarSystem;

namespace Pwe.Games.Cooking
{
    [CreateAssetMenu(fileName = "CookingLevels", menuName = "Cooking/Data/Levels", order = 0)]
    public class LevelsData : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> levels { get; private set; }

        [Serializable]
        public class LevelData
        {
            public List<ItemData> ingredients;
        }
    }
}
