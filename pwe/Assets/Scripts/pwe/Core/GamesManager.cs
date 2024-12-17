using System;
using UnityEngine;

namespace Pwe.Core
{
    public class GamesManager : MonoBehaviour
    {
        [SerializeField] Transform container;      
        [SerializeField] GameData[] all;
        public GameData[] All { get { return all; } }
        public static GamesManager Instance { get; private set; }

        GameObject newGame;
        GameData active;
        private void Awake()
        {
            Instance = this;
            InitGamesData();
        }
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
            YaguarLib.Events.Events.StopAllSounds();
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
        public GameData GetGame(GameData.GAMES game, string levelName = "main")
        {
            foreach (GameData g in all)
                if (g.game == game)
                    return g;
            Debug.LogError("No game: " + game);
            return null;
        }

        void GamePlayed()
        {
            ExitGame();
            Debug.Log("GamePlayed: " + active.game);
            if (active != null)
                active.LevelUp();
        }
        void InitGamesData()
        {
            foreach (GameData gameData in all)
                gameData.Init();
        }
        public void Reset()
        {
            PlayerPrefs.DeleteAll();
            foreach (GameData gameData in all)
                gameData.Reset();
            Events.Reset();
        }

    }
}
