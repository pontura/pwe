using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking.UI;
using Rive;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class Cooking : GameMain
    {
        public ItemData.Items itemDragging;
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
        [SerializeField] PieceToDrag pieceToDrag;
        [SerializeField] Transform dragContainer;
        PieceToDrag newPieceToDrag;

        [SerializeField] ButtonProgressBar buttonProgressBar;
        [SerializeField] string riveName;
        [SerializeField] RiveTexture riveTexture;

        Dictionary<string, int> ingredients;
        Dictionary<string, int> ingredientsAdded;
        int totalPieces;
        public CookingData Data { get { return (Game as CookingGame).CookingData;  } }

        [SerializeField] DragInputManager dragInputManager;

        int total;
        int num;
        List<ItemData> items;
        int itemID;
        string lastIngredient;
        [SerializeField] List<string> added;
        public override void OnInit()
        {
            riveTexture.Init(riveName, null);
            riveTexture.OnRiveEvent += RiveScreen_OnRiveEvent;

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
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            print("itemID " + itemID);
            switch(reportedEvent.Name)
            {
                case "up": 
                    if (itemID > items.Count) return; 
                    ChgangeNum(true); break;
                case "down": 
                    if (itemID < 0) return;
                    ChgangeNum(false); break;
                case "click":
                    itemDragging = items[itemID].item;
                    if (ingredientsAdded.ContainsKey(itemDragging.ToString()))
                    {
                        if (ingredientsAdded[itemDragging.ToString()] >= 10) return;
                    }
                    riveTexture.SetTrigger("remove_" + itemDragging);
                    InitDrag();  break;
            }
        }
        public void ResetDrag()
        {
            itemDragging = items[itemID].item;
            riveTexture.SetTrigger("add_" + itemDragging);
        }
        void ChgangeNum(bool up)
        {
            if(up) itemID++;
            else itemID--;
            
            if(itemID>items.Count-1) itemID = items.Count - 1;
            else if (itemID < 0) itemID = 0;
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
            {
                Events.OnTransition(OnTransitionDone);
            }
        }

        void OnTransitionDone()
        {
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

        void InitDrag()
        {
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
                    //if (ingredientsAdded[s] == ingredients[s])
                    //    pieces.OnIngredientReady(s);
                    //else 
                    if (ingredientsAdded[s] > ingredients[s])
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
