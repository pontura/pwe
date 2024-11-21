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

    }
}