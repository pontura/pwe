using Pwe.Core;
using Pwe.Games.Common;
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
        public void InitDrag()
        {
            newPieceToDrag = Instantiate(pieceToDrag, dragContainer);
            newPieceToDrag.Init(OnPieceToDragReady, mainPiece);           
        }
        void OnPieceToDragReady()
        {
            string ingredient = items[itemID].item.ToString();
            //mainPiece.InitIngredient("qty_" + ingredient, total);
            Invoke("InitPieceToDragDelayed", 0.1f);
            dragInputManager.ForceDrag(Input.mousePosition, newPieceToDrag);
        }
        void ResetIngredient(string ingredient)
        {
            mainPiece.InitIngredient("qty_" + ingredient, 0);
        }
        void CheckFinish()
        {
            print("DONE_____________" + ingredients.Keys.Count);
            if (ingredients.Keys.Count <= 0)
                print("DONE");
        }
        public void OnPieceAdded(string ingredient)
        {
            if (ingredients.ContainsKey(ingredient))
            {
                ingredientsAdded[ingredient]++;
                print(ingredient + " ingredientsAdded[ingredient] " + ingredientsAdded[ingredient]);
                int value = 0;
                foreach (string s in ingredientsAdded.Keys)
                {
                    int v = ingredientsAdded[s];
                    if (ingredientsAdded[s] == ingredients[s])
                        pieces.OnIngredientReady(s);

                    value += v;
                }
                if (ingredientsAdded[ingredient] <= ingredients[ingredient])
                {
                    buttonProgressBar.SetProgress(value, totalPieces);
                    numFeedback.Init(ingredientsAdded[ingredient]);
                }
            }
            if(state != states.done && buttonProgressBar.IsReady())
            {
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
