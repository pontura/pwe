using Rive;
using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class Cutscene : GameMain
    {
        [SerializeField] ButtonUI skipButton;
        [SerializeField] bool backToContinue;
        [SerializeField] string artboard = "outro";

        public override void OnInitialize()
        {
            skipButton.Init(Skip);
        }
        public override void OnInit()
        {
            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;

            if (artboard != "")
                GetRiveTexture().ActivateArtboard(artboard);

            base.OnInit();
        }
        public override void OnHide()
        {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        public virtual void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.Properties}");
            foreach(string key in reportedEvent.Properties.Keys)  { print("key " + key); }
        }
        void OnLoaded() { }
        void Skip()
        {
            if (backToContinue)
                Back();
            else
                Next();
        }
    }
}