using Pwe.Core;
using Pwe.Games.Cooking.UI;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;

            button.Init(Play);
            int level = 0;
            if (GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;
            items = cookingData.GetItems(level);

            GetRiveTexture().ActivateArtboard("intro");


            button.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);


            foreach (ItemData item in items)
            {
                print("item " + item.item.ToString() + " num: " + num);
                (Game as CookingGame).rive.SetNumberInArtboard("PizzaBase", "qty_" + item.item.ToString(), item.num);
            }
        }
        public override void OnHide()
        {
            print("___________OnHide");
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            if (clicked) return;
            clicked = true;
            StartCoroutine(Animate());
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
            Events.OnTransition(OnTransitionDone, "game");
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
