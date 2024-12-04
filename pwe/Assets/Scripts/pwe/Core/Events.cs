using UnityEngine;

namespace Pwe.Core
{
    public static class Events
    {
        public static System.Action<GameData.GAMES> OnPlayGame = delegate { };
        public static System.Action Reset = delegate { };
        public static System.Action ExitGame = delegate { };
        public static System.Action<System.Action, string> OnTransition = delegate { };
        public static System.Action OnWinParticles = delegate { };
        public static System.Action<ParticlesManager.types, Vector2> OnAddParticles = delegate { };
        public static System.Action GamePlayed = delegate { };
        public static System.Action<GameData.GAMES, int> GameLeveled = delegate { };
        public static System.Action<int> OnSayNumber = delegate { };
    }
}