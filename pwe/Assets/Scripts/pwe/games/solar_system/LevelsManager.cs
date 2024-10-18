using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] List<SpaceData> levels;

        [field:SerializeField] public int CurrentLevelIndex { get; private set; }

        void Start() {

        }

        public SpaceData GetCurrentLevel() {
            return levels[CurrentLevelIndex];
        }
    }
}