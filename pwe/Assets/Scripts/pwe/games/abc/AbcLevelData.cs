using System;
using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.Abc
{
    [CreateAssetMenu(fileName = "NewAbcLevelData", menuName = "Abc/Data/Level", order = 0)]
    public class AbcLevelData : Pwe.Games.Common.LevelData
    {
        [field: SerializeField] public LevelManager LevelPrefab { get; private set; }        
    }
}
