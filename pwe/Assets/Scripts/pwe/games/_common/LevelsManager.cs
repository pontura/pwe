using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pwe.Core;

namespace Pwe.Games.Common
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] protected GameData.GAMES gameType;
        [SerializeField] protected List<LevelData> levels;

        [field:SerializeField] public int CurrentLevelIndex { get; protected set; }

        public virtual event System.Action OnLevelCompleted;

        void Awake() {
            SetCurrentLevelIndex();            
        }

        protected virtual void SetCurrentLevelIndex() {
            if (GamesManager.Instance != null)
                CurrentLevelIndex = GamesManager.Instance.All[(int)gameType].level;
            Debug.Log("CurrentLevelIndex: " + CurrentLevelIndex);
        }

        public virtual LevelData InitLevel() {            
            return levels[CurrentLevelIndex];
        }                
    }
}