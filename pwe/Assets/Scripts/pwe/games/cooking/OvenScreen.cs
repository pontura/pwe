using Pwe.Core;
using Pwe.Games.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;
using static ItemData;

namespace Pwe.Games.Cooking
{
    public class OvenScreen : GameMain
    {
        states state;
        enum states
        {
            playing,
            playing_done,
            cooking,
            done
        }
        [SerializeField] ButtonUI btnNext;
        [SerializeField] ButtonUI btnPrev;
        [SerializeField] GameObject clockGO;
        [SerializeField] NumFeedback numFeedback;
        [SerializeField] BigNumberSignal bigNumberSignal;
        int num;
        int ovenDuration = 4;
        int totalNums = 10;
        float speedWhenDone = 60;
        float speed = 2;

        public override void OnInitialize()
        {
            if (GamesManager.Instance != null)
            {
                int level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;
                ovenDuration = Game.CookingData.GetLevelData(level).ovenDuration;
            }

            bigNumberSignal.Init(ovenDuration);
            btnNext.Init(NextClicked);
            btnPrev.Init(PrevClicked);
            btnPrev.SetInteraction(false);
        }
        public override void OnUpdate()
        {
            if (state == states.playing || state == states.playing_done)
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
            float _rot = Mathf.Lerp(clockGO.transform.localEulerAngles.z, _a, speed*Time.deltaTime);
            clockGO.transform.localEulerAngles = new Vector3(0, 0, _rot);
        }
        public override void OnInit()
        {
            state = states.playing;
            base.OnInit();
        }
        void NextClicked()
        {
            if (Ended()) return;
            if (state != states.playing) return;
            num++;
            numFeedback.Init(num);
            Events.OnSayNumber(num);
            if (num >= ovenDuration)
                StartCooking();
            SetButtonsState();
        }
        void PrevClicked()
        {
            if (Ended()) return;
            if (state != states.playing) return;
            num--;
            if (num < 0) num = 0;
            SetButtonsState();
        }
        void SetButtonsState()
        {
            if(state == states.playing)
                btnPrev.SetInteraction(true);
            if (num<=0)
                btnPrev.SetInteraction(false);
            else if (Ended())
            {
                state = states.playing_done;
                btnNext.SetInteraction(false);
                btnPrev.SetInteraction(false);
                YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.REWARD);
            }
        }
        bool Ended()
        {
            return (num >= ovenDuration);
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
            yield return new WaitForSeconds(2);
            Next();
        }
    }
}