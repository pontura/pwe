using Pwe.Core;
using System;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class Map : MainScreen
    {
        [SerializeField] ButtonUIText cookingButton; //TO-DO

        public override void OnInitialize()
        {
            cookingButton.Init(OnCook);
        }

        private void OnCook()
        {
            Events.OnPlayGame(GamesManager.GAMES.COOKING);
        }
    }
}
