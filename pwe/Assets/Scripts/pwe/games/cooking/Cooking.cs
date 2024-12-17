using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking.UI;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;
using YaguarLib.Audio;
using YaguarLib.Xtras;
using static Pwe.Games.Cooking.CookingData;

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
        [SerializeField] Transform[] hints;

        [SerializeField] CookingMenuUI menu;

        [SerializeField] Transform mainPieceContainer;
        [SerializeField] NumFeedback numFeedback;
        [SerializeField] CookingMainPiece mainPiece;
        [SerializeField] PieceToDrag pieceToDrag;
        [SerializeField] Transform dragContainer;
        PieceToDrag newPieceToDrag;

        [SerializeField] ButtonProgressBar buttonProgressBar;

        Dictionary<string, int> ingredients;
        Dictionary<string, int> ingredientsAdded;
        int totalPieces;

        public CookingData cookingData;
        [SerializeField] DragInputManager dragInputManager;

        int total;
        int num;
        List<ItemData> items;
        int itemID;
        string lastIngredient;
        [SerializeField] List<string> added;
        int hintID = 0;

        int level = 0;

        public override void OnInit()
        {
            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("background_music_ingame").clip, AudioManager.channels.MUSIC);
            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;
            GetRiveTexture().ActivateArtboard("game");


            buttonProgressBar.Init(NextClicked);
            buttonProgressBar.SetProgress(false);
            buttonProgressBar.SetInteraction(false);
            added = new List<string>();
           

            if(GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level.level;

            items = new List<ItemData>();
            items = (Game as CookingGame).CookingData.GetItems(level);

            ingredients = new Dictionary<string, int>();
            ingredientsAdded = new Dictionary<string, int>();
            
            if(mainPiece.WasInit())
            {
                mainPiece.transform.SetParent(mainPieceContainer);
                mainPiece.transform.localScale = Vector3.one;
                mainPiece.transform.localEulerAngles = Vector3.zero;
                mainPiece.transform.localPosition = Vector3.zero;
                if (mainPiece.GetComponent<Animation>() != null)
                    Destroy(mainPiece.GetComponent<Animation>());
                if (mainPiece.GetComponent<ResolutionFixer>() != null)
                    Destroy(mainPiece.GetComponent<ResolutionFixer>());
            } else
                mainPiece.Init(cookingData.Part);

            SetMenu();
            if (HintsAvailable() && hintID == 0)
            {
                Events.OnHint(hints[0].transform.position);
                hintID++;
            }
            StartCoroutine(AnimIngredientsOn());
        }
        bool HintsAvailable()
        {
            return (level <= 0);
        }
        IEnumerator AnimIngredientsOn()
        {
            yield return new WaitForSeconds(0.25f);
            foreach (ItemData item in items)
            {
                yield return new WaitForSeconds(0.1f);
                print("item " + item.item.ToString() + " num: " + num);
                totalPieces += item.num;
                ingredients.Add(item.item.ToString(), item.num);
                ingredientsAdded.Add(item.item.ToString(), 0);
                GetRiveTexture().SetTrigger("game", item.item.ToString() + "_on");
            }
            buttonProgressBar.SetProgress(0, totalPieces);
        }
        public override void OnHide()
        {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            switch (reportedEvent.Name)
            {
                case "up":
                    if (HintsAvailable())
                    {
                        if (hintID == 1) hintID++;
                        Events.OnHideAllHints();
                    }
                    if (itemID >= items.Count-1) return; 
                    GetRiveTexture().SetTrigger("game", "scroll_up");
                    ChgangeNum(true); break;
                case "down":
                    if (HintsAvailable())
                    {
                        if (hintID == 1) hintID++;
                        Events.OnHideAllHints();
                    }
                    if (itemID <= 0) return; 
                    GetRiveTexture().SetTrigger("game", "scroll_down");
                    ChgangeNum(false); break;
                case "click":
                    itemDragging = items[itemID].item;
                    if (ingredientsAdded.ContainsKey(itemDragging.ToString()))
                    {
                        if (ingredientsAdded[itemDragging.ToString()] >= 10) return;
                    }
                    YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("ingredient_snap").clip, AudioManager.channels.GAME);
                    GetRiveTexture().SetTriggerInArtboard("bowl_" + itemDragging, "remove");
                    InitDrag();  break;
            }
        }
        public void ResetDrag()
        {
            itemDragging = items[itemID].item;
            GetRiveTexture().SetTriggerInArtboard("bowl_" + itemDragging, "add");
            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("ingredient_respawn").clip, AudioManager.channels.GAME);
        }
        void ChgangeNum(bool up)
        {
            if(up) itemID++;
            else itemID--;

            if (itemID > items.Count - 1) itemID = items.Count - 1;
            else if (itemID < 0) itemID = 0;
            else
            {
                GetRiveTexture().SetTriggerInArtboard("bowl_" + items[itemID].item, "plate_jump");
                YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("bowl_drag").clip, AudioManager.channels.GAME);
            }
        }
        void SetMenu()
        {
            menu.Init(items, cookingData);
        }
        private void NextClicked()
        {
            if (buttonProgressBar.IsReady())
            {
                if (cookingData.Part == "pizza")
                    Events.OnTransition(OnTransitionDone, "oven");
                else
                    Events.OnTransition(OnTransitionDone, "outro");
            }
        }

        void OnTransitionDone()
        {

            YaguarLib.Events.Events.StopChannel( AudioManager.channels.MUSIC);
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
            Events.OnAddParticles(ParticlesManager.types.pick, Input.mousePosition, itemDragging.ToString());
            newPieceToDrag = Instantiate(pieceToDrag, dragContainer);
            int pieceToGetID = 10;

            if (!ingredientsAdded.ContainsKey(itemDragging.ToString()))
                ingredientsAdded.Add(itemDragging.ToString(), 0);

            if (ingredientsAdded.ContainsKey(itemDragging.ToString()))
                pieceToGetID = pieceToGetID - ingredientsAdded[itemDragging.ToString()];

            newPieceToDrag.Init(OnPieceToDragReady, mainPiece, pieceToGetID);           
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

            if (ingredients[ingredient] == 0)
            {
                ingredientsAdded[ingredient]++;
                YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("ingredient_other").clip, AudioManager.channels.GAME);
                return;
            }

            if (ingredients.ContainsKey(ingredient) && ingredients[ingredient]>0)
            {
                ingredientsAdded[ingredient]++;
                int value = 0;
                foreach (string s in ingredientsAdded.Keys)
                {
                    int v = ingredientsAdded[s];
                    if (ingredients[s]>0 &&  ingredientsAdded[s] == ingredients[s])
                    {
                        menu.OnIngredientDone(s);
                        if ( HintsAvailable() && hintID == 1)
                        {
                            print("Hint");
                            Events.OnHint(hints[1].transform.position);
                            hintID++;
                        }
                        YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("ingredient_done").clip, AudioManager.channels.GAME);
                    } else  if (ingredientsAdded[s] > ingredients[s])
                    {
                        v = ingredients[s];
                    }
                    value +=v;
                }
                if (ingredientsAdded[ingredient] <= ingredients[ingredient])
                {
                    YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("ingredient_done").clip, AudioManager.channels.GAME);
                    buttonProgressBar.SetProgress(value, totalPieces);
                    numFeedback.Init(ingredientsAdded[ingredient]);
                    numFeedback.transform.position = Input.mousePosition;
                }
            }
            if(state != states.done && buttonProgressBar.IsReady())
            {
                YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("recipe_done").clip, AudioManager.channels.GAME);
                List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
                foreach (string s in ingredients.Keys)
                {
                    if (ingredients[s] > 0)
                    { 
                        foreach (IngredientsData.IngredientData iData in (Game as CookingGame).CookingData.GetIngredientsData())
                        {
                            if (s == iData.item.ToString())
                                colors.Add(iData.color);
                        }
                    }
                }
                Events.OnWinParticles(colors);
                buttonProgressBar.SetInteraction(true);
                state = states.done;
            }
        }
        public bool CanMove()
        {
            return true;
        }
    }
}
