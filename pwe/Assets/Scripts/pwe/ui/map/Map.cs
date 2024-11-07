using Pwe.Core;
using System;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class Map : MainScreen
    {
        [SerializeField] ButtonUIText[] games; //TO-DO

        private void Awake()
        {
            Events.Reset += Reset;
            Events.GameLeveled += GameLeveled;
        }
        private void OnDestroy()
        {
            Events.Reset -= Reset;
            Events.GameLeveled -= GameLeveled;
        }
       
        public override void OnInitialize()
        {
            SetData();
        }
        void SetData()
        {
            for (int a = 0; a < games.Length; a++)
            {
                games[a].Init(OnClick, a);
                GameData gameData = GamesManager.Instance.All[a];
                string text = gameData.game.ToString() + " (" + gameData.level + ")";
                games[a].SetText(text);
            }
        }
        private void OnClick(int id)
        {
            switch(id)
            {
                case 0:
                    Events.OnPlayGame(GameData.GAMES.COOKING);
                    break;
                case 1:
                    Events.OnPlayGame(GameData.GAMES.PHOTOS);
                    break;
            }
        }
        private void GameLeveled(GameData.GAMES game, int level)
        {
            SetData();
        }

        private void Reset()
        {
            SetData();
        }
    }
}
