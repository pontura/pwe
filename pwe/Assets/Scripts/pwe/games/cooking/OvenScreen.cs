using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class OvenScreen : GameMain
    {
        [SerializeField] ButtonUI btnNext;
        [SerializeField] ButtonUI btnPrev;
        [SerializeField] GameObject clockGO;
        int num;
        int finalNum = 4;
        int totalNums = 10;

        public override void OnInitialize()
        {
            btnNext.Init(NextClicked);
            btnPrev.Init(PrevClicked);
        }
        public override void OnUpdate()
        {
            float _a = (float)num * 360 / (float)totalNums;
            float _rot = Mathf.Lerp(clockGO.transform.localEulerAngles.z, _a, 0.05f);
            clockGO.transform.localEulerAngles = new Vector3(0, 0, _rot);
        }
        public override void OnInit()
        {
            base.OnInit();
        }
        void NextClicked()
        {
            num++;
            if (num >= finalNum)
                Go();
        }
        void PrevClicked()
        {
            num--;
            if (num < 0) num = 0;
        }
        void Go()
        {
            Next();
        }
    }
}