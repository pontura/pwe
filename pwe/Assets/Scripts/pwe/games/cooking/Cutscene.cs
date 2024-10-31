using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class Cutscene : GameMain
    {
        [SerializeField] ButtonUI skipButton;
        [SerializeField] bool backToContinue;

        public override void OnInitialize()
        {
            skipButton.Init(Skip);
        }
        public override void OnInit()
        {
            base.OnInit();
        }
        void Skip()
        {
            if (backToContinue)
                Back();
            else
                Next();
        }
    }
}