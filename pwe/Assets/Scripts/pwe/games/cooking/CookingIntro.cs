using Pwe.Core;
using Pwe.Games.Cooking.UI;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YaguarLib.Audio;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class CookingIntro : GameMain
    {
        [SerializeField] CookingData cookingData;
        [SerializeField] Animation anim;
        [SerializeField] float autoskipInSeconds;
        [SerializeField] CookingMenuUI menu;
        [SerializeField] ButtonUI button;
        List<ItemData> items;
        ItemData itemData;
        int itemID = 0;
        int num = 0;
        string ingredient;
        float speed = 0.4f;

        public override void OnInit()
        {
            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("intro_cutscene_music").clip, AudioManager.channels.MUSIC);
            print("___________1");
            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;

            button.Init(Play);

            GetRiveTexture().ActivateArtboard("intro");

            button.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
        }
        void InitLevel(string levelName)
        {
            int level = 0;
            if (GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).GetLevelData(levelName).level;
            items = cookingData.GetItems(level);
            foreach (ItemData item in items)
            {
                print("item " + item.item.ToString() + " num: " + num);
                (Game as CookingGame).rive.SetNumberInArtboard("PizzaBase/" + item.item.ToString(), "qty", item.num);
            }
        }
        public override void OnHide()
        {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            
            if (reportedEvent.Name.Contains("food"))
            {
                if (clicked) return;

                YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("intro_cutscene_fx").clip, AudioManager.channels.UI);
                YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("button_continue").clip, AudioManager.channels.GAME);


                switch (reportedEvent.Name)
                {
                    case "food2": 
                        cookingData.Part = "cake"; 
                        YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("vo_cake").clip, AudioManager.channels.VOICES);
                        break;
                    case "food3": 
                        cookingData.Part = "waffle"; 
                        YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("vo_waffle").clip, AudioManager.channels.VOICES);
                        break;
                    default: 
                        cookingData.Part = "pizza"; 
                        YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("vo_pizza").clip, AudioManager.channels.VOICES);
                        break;
                }

                InitLevel(cookingData.Part);
                clicked = true;
                StartCoroutine(Animate());
            }
            else if (reportedEvent.Name.Contains("audio_"))
            {
                string audioName = reportedEvent.Name.Remove(0, 6); // borra: "audio_"
                foreach (string channel in reportedEvent.Properties.Keys)
                {
                    print("Play cutscene audio  key " + audioName);
                    AudioClip audioClip = Game.Sounds.GetClip(audioName).clip;
                    if (audioClip != null)
                    {
                        YaguarLib.Events.Events.PlayGenericSound(audioClip, AudioManager.channels.GAME);
                    }
                    else
                    {
                        Debug.LogError("No generic audio named: " + audioName);
                    }
                }
            }
        }
        bool clicked;
        IEnumerator Animate()
        {            

            yield return new WaitForSeconds(4.5f);

            anim.Play("cutscene_2");
            (Game as CookingGame).rive.SetTrigger("intro", "next");
            menu.gameObject.SetActive(true);
            menu.Init(items, cookingData);

            yield return new WaitForSeconds(1);

            button.gameObject.SetActive(true);
        }
        private void Play()
        {
            if(cookingData.Part == "pizza" || cookingData.Part == "waffle")
                Events.OnTransition(OnTransitionDone, "game");
            else
                Events.OnTransition(OnTransitionDone, "oven");
        }
        void OnTransitionDone()
        {
            Next();
        }

        void OnMainPieceLoaded()
        {
            foreach (ItemData itemData in items)
            {
                string s = itemData.item.ToString();
              //  mainPiece.InitIngredient("qty_" + s, itemData.num);
            }
        }
        void NextPiece()
        {
            print("NextPiece");
            if (num >= itemData.num)
            {
                SetNewIngredient();
            }                
            else
            {
                num++;
              //  mainPiece.Add();
                Invoke("NextPiece", speed);
            }
        }
        void ResetOtherIngredients(string ingredient)
        {
            foreach (ItemData itemData in items)
            {
                string s = itemData.item.ToString();
                if (s != ingredient)
                    ResetIngredient(s);// RESET
            }
        }
        void ResetIngredient(string ingredient)
        {
         //   mainPiece.InitIngredient("qty_" + ingredient, 0);
        }
        void SetNewIngredient()
        {
            if (itemID >= items.Count)
                Done();
            else
            {
                itemData = items[itemID];
                ingredient = itemData.item.ToString();
                num = 0;
                itemID++;
                ResetOtherIngredients(ingredient); 
              //  mainPiece.InitIngredient("qty_" + ingredient, itemData.num);
                Invoke("NextPiece", speed);
            }
        }
        void Done()
        {

        }
    }
}
