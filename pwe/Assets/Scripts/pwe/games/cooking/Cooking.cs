using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class Cooking : GameMain
    {
        public states state;
        public enum states
        {
            playing,
            done
        }
        //menu items:
        [SerializeField] CookingMenuUI menu;
        [SerializeField] Transform menuContainer;

        [SerializeField] NumFeedback numFeedback;
        [SerializeField] CookingMainPiece mainPiece;
        [SerializeField] CookingPieces pieces;
        [SerializeField] PieceToDrag pieceToDrag;
        [SerializeField] Transform dragContainer;
        PieceToDrag newPieceToDrag;

        [SerializeField] ButtonProgressBar buttonProgressBar;

        Dictionary<string, int> ingredients;
        Dictionary<string, int> ingredientsAdded;
        int totalPieces;

        [SerializeField] DragInputManager dragInputManager;

        int total;
        int num;
        List<ItemData> items;
        int itemID;
        string lastIngredient;
        [SerializeField] List<string> added;
        public override void OnInit()
        {
            buttonProgressBar.Init(NextClicked);
            buttonProgressBar.SetProgress(false);
            buttonProgressBar.SetInteraction(false);
            added = new List<string>();
            int level = 0;

            if(GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;

            items = new List<ItemData>();
            items = (Game as CookingGame).CookingData.GetItems(level);
            ingredients = new Dictionary<string, int>();
            ingredientsAdded = new Dictionary<string, int>();
            foreach (ItemData item in items)
            {
                totalPieces += item.num;
                ingredients.Add(item.item.ToString(), item.num);
                ingredientsAdded.Add(item.item.ToString(), 0);
            }
            mainPiece.Init(InitIngredient);
            buttonProgressBar.SetProgress(0, totalPieces);

            SetMenu();
        }
        void SetMenu()
        {
            menu.transform.SetParent(menuContainer);
            menu.transform.localPosition = Vector3.zero;
            menu.transform.localScale = Vector3.one;
        }
        private void NextClicked()
        {
            if (buttonProgressBar.IsReady())
                Next();
        }

        void InitIngredient()
        {
            num = 0;
            state = states.playing;
            total = items[itemID].num;

            string ingredient = items[itemID].item.ToString();

            if(lastIngredient != null)      ResetIngredient(lastIngredient);
            else                            ResetOtherIngredients(ingredient);

            lastIngredient = ingredient;

           

            pieces.Initialize(this, items, mainPiece);
        }
        void ResetOtherIngredients(string ingredient)
        {
            foreach (ItemData itemData in items)
            {
                string s = itemData.item.ToString();
                if(s != ingredient)
                    ResetIngredient(s);// RESET
            }
        }

        ItemData.Items itemDragging;
        public void InitDrag(ItemData.Items itemDragging)
        {
            this.itemDragging = itemDragging;
            newPieceToDrag = Instantiate(pieceToDrag, dragContainer);
            newPieceToDrag.Init(OnPieceToDragReady, mainPiece);           
        }
        void OnPieceToDragReady()
        {
            string ingredient = itemDragging.ToString();
           // mainPiece.InitIngredient("qty_" + ingredient, total);
            dragInputManager.ForceDrag(Input.mousePosition, newPieceToDrag);
        }
        void ResetIngredient(string ingredient)
        {
            mainPiece.InitIngredient("qty_" + ingredient, 0);
        }
        public void OnPieceAdded(string ingredient)
        {
            if (ingredients.ContainsKey(ingredient))
            {
                ingredientsAdded[ingredient]++;
                int value = 0;
                foreach (string s in ingredientsAdded.Keys)
                {
                    int v = ingredientsAdded[s];
                    if (ingredientsAdded[s] == ingredients[s])
                        pieces.OnIngredientReady(s);
                    else if (ingredientsAdded[s] > ingredients[s])
                        v = ingredients[s];
                    value +=v;
                }
                if (ingredientsAdded[ingredient] <= ingredients[ingredient])
                {
                    buttonProgressBar.SetProgress(value, totalPieces);
                    numFeedback.Init(ingredientsAdded[ingredient]);
                }
            }
            if(state != states.done && buttonProgressBar.IsReady())
            {
                Events.OnWinParticles();
                buttonProgressBar.SetInteraction(true);
                state = states.done;
                YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.REWARD);
            }
        }
        public bool CanMove()
        {
            return true;
        }
    }
}
