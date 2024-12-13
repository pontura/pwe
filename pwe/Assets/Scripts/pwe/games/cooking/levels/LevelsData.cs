using System;
using UnityEngine;
using System.Collections.Generic;
using Pwe.Games.SolarSystem;

namespace Pwe.Games.Cooking
{
    [CreateAssetMenu(fileName = "CookingLevels", menuName = "Cooking/Data/Levels", order = 0)]
    public class LevelsData : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> pizza { get; private set; }
        [field: SerializeField] public List<LevelData> cake { get; private set; }
        [field: SerializeField] public List<LevelData> waffle { get; private set; }

        public List<LevelData> GetPart(string partName)
        {
            switch(partName)
            {
                case "pizza": return pizza;
                case "cake": return cake;
                default: return waffle;
            }
        }

        [Serializable]
        public class LevelData
        {
            public List<ItemData> ingredients;
        }
    }
}
