namespace Pwe.Core
{
    public static class Events
    {
        public static System.Action<GamesManager.GAMES> OnPlayGame = delegate { };
        public static System.Action ExitGame = delegate { };
        public static System.Action<int> OnSayNumber = delegate { };
    }
}