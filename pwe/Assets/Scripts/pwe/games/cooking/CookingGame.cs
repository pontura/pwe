using Pwe.Core;
using Pwe.Games.Cooking;
using Rive;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pwe.Games.Cooking
{
    public class CookingGame : Game
    {
        public CookingData CookingData;

        public override void AllRivesLoaded()
        {

            rive.Init(asset);
        //}
        //void OnLoaded()
        //{
            LoadAllArtBoards();
            InitScreens();
            rive.OnRiveEvent += RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");

            //// Access specific event properties
            //if (reportedEvent.Name.StartsWith("click"))
            //{
            //    var click = reportedEvent["click"];
            //    var type = reportedEvent["type"];
            //    Debug.Log($"click: {click}");
            //    Debug.Log($"type: {type}");
            //}
            switch(reportedEvent.Name)
            {
                case "food2":
                    GameMain g1 = screens[1];
                    GameMain g2 = screens[2];
                    screens[1] = g2;
                    screens[2] = g1;
                    break;
                case "food3":
                    screens.RemoveAt(2);
                    break;

            }
        }
    }
}