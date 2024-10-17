using Pwe.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class OvenScreen : GameMain
    {
        states state;
        enum states
        {
            playing,
            cooking,
            done
        }
        [SerializeField] ButtonUI btnNext;
        [SerializeField] ButtonUI btnPrev;
        [SerializeField] GameObject clockGO;
        int num;
        int finalNum = 4;
        int totalNums = 10;
        float speedWhenDone = 50;

        public override void OnInitialize()
        {
            btnNext.Init(NextClicked);
            btnPrev.Init(PrevClicked);
            btnPrev.SetInteraction(false);
        }
        public override void OnUpdate()
        {
            if (state == states.playing)
                UpdateClicks();
            else if (state == states.cooking)
                UpdateCooking();
        }
        void UpdateCooking()
        {
            Vector3 rot = clockGO.transform.localEulerAngles;
            rot.z -= speedWhenDone * Time.deltaTime;
            if (rot.z <= 0)
            {
                rot.z = 0;
                Done();
            }
            clockGO.transform.localEulerAngles = rot;
        }
        void UpdateClicks()
        {
            float _a = (float)num * 360 / (float)totalNums;
            float _rot = Mathf.Lerp(clockGO.transform.localEulerAngles.z, _a, Time.deltaTime);
            clockGO.transform.localEulerAngles = new Vector3(0, 0, _rot);
        }
        public override void OnInit()
        {
            state = states.playing;
            base.OnInit();
        }
        void NextClicked()
        {
            if (num >= finalNum) return;
            if (state != states.playing) return;
            num++;
            Events.OnSayNumber(num);
            if (num >= finalNum)
                StartCooking();
            SetButtonsState();
        }
        void PrevClicked()
        {
            if (num >= finalNum) return;
            if (state != states.playing) return;
            num--;
            if (num < 0) num = 0;
            SetButtonsState();
        }
        void SetButtonsState()
        {
            btnPrev.SetInteraction(true);
            if (num<=0)
                btnPrev.SetInteraction(false);
            else if (num >= finalNum)
                btnNext.SetInteraction(false);
        }
        void StartCooking()
        {
            Invoke("StartCookingDone", 2);
        }
        void StartCookingDone()
        {
            state = states.cooking;
        }
        void Done()
        {
            state = states.done;
            StartCoroutine(NextIngredient());
        }
        IEnumerator NextIngredient()
        {
            YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.REWARD);            
            yield return new WaitForSeconds(2);
            Next();
        }
    }
}