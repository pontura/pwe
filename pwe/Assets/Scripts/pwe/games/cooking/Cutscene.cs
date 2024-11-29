using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class Cutscene : GameMain
    {
        [SerializeField] RiveRawImage riveRawImage;
        [SerializeField] ButtonUI skipButton;
        [SerializeField] bool backToContinue;
        [SerializeField] string riveName = "Cooking/cutscenes/outro.riv";

        public override void OnInitialize()
        {
            skipButton.Init(Skip);
        }
        public override void OnInit()
        {
            if(riveRawImage != null) // TO-DO
                riveRawImage.Init(riveName, OnLoaded);
            base.OnInit();
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