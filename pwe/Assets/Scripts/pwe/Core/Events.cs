﻿using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Core
{
    public static class Events
    {
        public static System.Action OnHideAllHints = delegate { };
        public static System.Action<Vector2> OnHint = delegate { };
        public static System.Action<GameData.GAMES> OnPlayGame = delegate { };
        public static System.Action Reset = delegate { };
        public static System.Action ExitGame = delegate { };
        public static System.Action<System.Action, string> OnTransition = delegate { };
        public static System.Action<List<Color>, Vector2> OnWinParticles = delegate { };
        public static System.Action<ParticlesManager.types, Vector2, string> OnAddParticles = delegate { };
        public static System.Action GamePlayed = delegate { };
        public static System.Action<GameData.GAMES, int> GameLeveled = delegate { };
        public static System.Action<int> OnSayNumber = delegate { };
    }
}