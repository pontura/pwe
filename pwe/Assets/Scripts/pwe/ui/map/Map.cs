using Pwe.Core;
using System;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class Map : MainScreen
    {
        [SerializeField] ButtonUIText[] games; //TO-DO

        public override void OnInitialize()
        {
            for(int a  = 0; a< games.Length; a++)
                games[a].Init(OnClick, a);
        }
        private void OnClick(int id)
        {
            switch(id)
            {
                case 0:
                    Events.OnPlayGame(GamesManager.GAMES.COOKING);
                    break;
                case 1:
                    Events.OnPlayGame(GamesManager.GAMES.PHOTOS);
                    break;
            }
        }
    }
}
