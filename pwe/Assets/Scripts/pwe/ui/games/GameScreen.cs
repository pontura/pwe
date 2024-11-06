using Pwe.Core;
using System;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class GameScreen : MainScreen
    {
        [SerializeField] ButtonUIText done;
        [SerializeField] ButtonUIText back;

        public override void OnInitialize()
        {
            done.Init(Done);
            back.Init(Back);
        }

        private void Back()
        {
            Core.Events.ExitGame();
        }

        private void Done()
        {
            Core.Events.GamePlayed();
        }
    }
}
