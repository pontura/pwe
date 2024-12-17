using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pwe.Core;

namespace Pwe.Games.Abc
{
    public class LevelsManager : Pwe.Games.Common.LevelsManager
    {
        void Awake() {
            SetCurrentLevelIndex();
        }

        public void SetLevelCompleted() {
            CurrentLevelIndex++;
            if (CurrentLevelIndex >= levels.Count) {
                CurrentLevelIndex = 0;
               // GamesManager.Instance.All[(int)(int)gameType].level = 0;
            }
        }
    }
}