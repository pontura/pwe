using Pwe.Core;
using Pwe.Games.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.Audio;
using YaguarLib.UI;
using YaguarLib.Xtras;
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
        [SerializeField] Animation anim;
        [SerializeField] ButtonUI btnDone;
        [SerializeField] ButtonUI btnNext;
        [SerializeField] ButtonUI btnPrev;
        [SerializeField] GameObject clockGO;
        [SerializeField] GameObject smoke;
        [SerializeField] TMPro.TMP_Text field;
        [SerializeField] CookingMainPiece mainPiece;
        [SerializeField] Transform mainPieceContainer;
        [SerializeField] Transform[] hints;
        int num;
       // int ovenDuration = 4;
        int totalNums = 10;
        float speedWhenDone = 70;
        float speed = 8;

        public override void OnInitialize()
        {
            if (GamesManager.Instance != null)
            {
                int level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;
            }
            smoke.SetActive(false);
            btnDone.Init(OnDoneClicked);
            btnNext.Init(NextClicked);
            btnPrev.Init(PrevClicked);
        }
        public override void OnInit()
        {
            base.OnInit();
            mainPiece.transform.SetParent(mainPieceContainer);
            mainPiece.transform.localScale = Vector3.one;
            mainPiece.transform.localPosition = Vector3.zero;
            if(mainPiece.GetComponent<Animation>() != null)
                Destroy(mainPiece.GetComponent<Animation>());
            if (mainPiece.GetComponent<ResolutionFixer>() != null)
                Destroy(mainPiece.GetComponent<ResolutionFixer>());
            state = states.playing;

            GetRiveTexture().ActivateArtboard("oven");

            Invoke("Delayed", 0.5f);
        }
        void Delayed()
        {
            Events.OnHint(hints[0].transform.position);
            anim.Play("close");
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
            print("rot.z " + rot.z + "rot: "+(rot.z/ 34) );
            int f =  (int)Mathf.Ceil(rot.z/34);
            field.text = f.ToString();

            clockGO.transform.localEulerAngles = rot;
        }
        void UpdateClicks()
        {
            float _a = (float)num * 300 / (float)totalNums;
            float _rot = Mathf.Lerp(clockGO.transform.localEulerAngles.z, _a, speed*Time.deltaTime);
            clockGO.transform.localEulerAngles = new Vector3(0, 0, _rot);
        }
        void OnDoneClicked()
        {
            if (num == 0) return;
            smoke.SetActive(true);
            state = states.playing_done;
            btnNext.SetInteraction(false);
            btnPrev.SetInteraction(false);
            YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.TAP);
            StartCooking();
        }
        void NextClicked()
        {
            if (num == 10) return;
            if (state != states.playing) return;
            num++;
            OnFeedback();
        }
        void PrevClicked()
        {
            if (num < 1) return;
            num = 1;
            if (state != states.playing) return;
            num--;
            OnFeedback();
        }
        void OnFeedback()
        {
            YaguarLib.Events.Events.OnPlaySound(AudioManager.types.TAP);
            field.text = num.ToString();
            if(num>0)
                Events.OnSayNumber(num);
            SetButtonsState();

        }
        void SetButtonsState()
        {
            btnNext.SetInteraction(true);
            btnPrev.SetInteraction(true);
            if (num>=10)
                btnNext.SetInteraction(false);
            if (num<=0)
                btnPrev.SetInteraction(false);
        }
      
        void StartCooking()
        {
            Invoke("StartCookingDone", 0.25f);
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
            YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.FINISH);
            yield return new WaitForSeconds(0.25f);
            anim.Play("ready");
            yield return new WaitForSeconds(1f);
            // Events.OnTransition(OnTransitionDone, "outro");
            OnTransitionDone();
        }
        void OnTransitionDone()
        {
            Next();
        }
    }
}