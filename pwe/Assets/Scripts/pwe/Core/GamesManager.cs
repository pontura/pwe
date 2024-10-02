using System;
using UnityEngine;

namespace Pwe.Core
{
    public class GamesManager : MonoBehaviour
    {
        [SerializeField] Transform container;
        public enum GAMES
        {
            COOKING,
            PHOTOS
        }
        [SerializeField] GameData[] all;
        [Serializable]
        public class GameData
        {
            public GAMES game;
            public GameObject gameGO;
        }
        GameObject newGame;
        private void Start()
        {
            Events.OnPlayGame += OnPlayGame;
            Events.ExitGame += ExitGame;
        }
        private void OnDestroy()
        {
            Events.OnPlayGame -= OnPlayGame;
            Events.ExitGame -= ExitGame;
        }
        private void ExitGame()
        {
            Destroy(newGame.gameObject);
        }
        private void OnPlayGame(GAMES _game)
        {
            GameObject g = GetGame(_game);
            newGame = Instantiate(g, container);
            newGame.SetActive(true);
          //  newGame.SendMessage("OnInit");
        }
        GameObject GetGame(GAMES game)
        {
            foreach (GameData g in all)
                if (g.game == game)
                    return g.gameGO;
            Debug.LogError("No game: " + game);
            return null;
        }
    }
}
