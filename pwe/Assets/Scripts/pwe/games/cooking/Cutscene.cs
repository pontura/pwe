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
            if (artboard != "")
                GetRiveTexture().ActivateArtboard(artboard);

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