using Pwe.Core;
using Pwe.Games.Cooking.UI;
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
            button.Init(Play);
            int level = 0;
            if (GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;
            items = cookingData.GetItems(level);

            GetRiveTexture().ActivateArtboard("intro");
            StartCoroutine(Animate());
        }
       
        IEnumerator Animate()
        {
            menu.Init(items, cookingData);
            menu.SetType(true);

            button.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            foreach (ItemData item in items) 
                (Game as CookingGame).rive.SetNumber("intro", item.item.ToString(), item.num);

            yield return new WaitForSeconds(3);

            anim.Play("cutscene_2");
            (Game as CookingGame).rive.SetTrigger("intro", "next");
            menu.gameObject.SetActive(true);

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
