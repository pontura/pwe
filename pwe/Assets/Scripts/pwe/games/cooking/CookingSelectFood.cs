using Pwe.Core;
using Pwe.Games.Cooking.UI;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.UI;
using static ItemData;
using static Pwe.Games.Compare.ItemData;

namespace Pwe.Games.Cooking
{
    public class CookingSelectFood : GameMain
    {
        public override void OnInit()
        {
            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;
            GetRiveTexture().ActivateArtboard("mainMenu");
        }
        public override void OnHide()
        {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            Invoke("OnTransitionDone", 2.5f);
        }
        //private void Clicked()
        //{
        //    Events.OnTransition(OnTransitionDone, "intro");
        //}
        void OnTransitionDone()
        {
            Next();
        }
    }

}