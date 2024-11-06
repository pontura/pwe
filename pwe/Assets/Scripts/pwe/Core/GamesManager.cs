using System;
using UnityEngine;

namespace Pwe.Core
{
    public class GamesManager : MonoBehaviour
    {
        [SerializeField] Transform container;      
        [SerializeField] GameData[] all;

        GameObject newGame;
        GameData active;
        private void Start()
        {
            Events.OnPlayGame += OnPlayGame;
            Events.ExitGame += ExitGame;
            Events.GamePlayed += GamePlayed;
        }
        private void OnDestroy()
        {
            Events.OnPlayGame -= OnPlayGame;
            Events.ExitGame -= ExitGame;
            Events.GamePlayed -= GamePlayed;
        }
        private void ExitGame()
        {
            Destroy(newGame.gameObject);
        }
        private void OnPlayGame(GameData.GAMES game)
        {
            active = GetGame(game);
            GameObject g = active.gameGO;
            newGame = Instantiate(g, container);
            newGame.SetActive(true);
          //  newGame.SendMessage("OnInit");
        }
        GameData GetGame(GameData.GAMES game)
        {
            foreach (GameData g in all)
                if (g.game == game)
                    return g;
            Debug.LogError("No game: " + game);
            return null;
        }

        void GamePlayed()
        {
            Debug.Log("GamePlayed: " + active.game);
            if (active != null)
                active.LevelUp();
        }

    }
}
