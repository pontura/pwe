using Pwe.Core;
using Pwe.Games.Cooking.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class CookingIntro : GameMain
    {
        [SerializeField] float autoskipInSeconds;
        [SerializeField] CookingMenuUI menu;
        [SerializeField] ButtonUI button;
        [SerializeField] CookingMainPiece mainPiece;
        List<ItemData> items;
        ItemData itemData;
        int itemID = 0;
        int num = 0;
        string ingredient;
        float speed = 0.4f;
        public override void OnInit()
        {
            int level = 0;

            if (GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;

            items = (Game as CookingGame).CookingData.GetItems(level);
            menu.Init(items);
          
            mainPiece.Init(OnMainPieceLoaded);
            if(autoskipInSeconds > 0 )
            {
                button.gameObject.SetActive(false);
                Invoke("Play", 2);
            }
            else
            {
                button.Init(Play);
            }
        }
        private void Play()
        {
            Next();
        }

        void OnMainPieceLoaded()
        {
            //CancelInvoke();
            //Invoke("SetNewIngredient", 0.5f);
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
                mainPiece.Add();
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
            mainPiece.InitIngredient("qty_" + ingredient, 0);
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
                mainPiece.InitIngredient("qty_" + ingredient, itemData.num);
                Invoke("NextPiece", speed);
            }
        }
        void Done()
        {

        }
    }
}
