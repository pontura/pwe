using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pwe.Games.Cooking.CookingData;

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
        [SerializeField] CookingData cookingData;
        [SerializeField] CookingMenuUI menu;
        [SerializeField] NumFeedback numFeedback;
        [SerializeField] CookingMainPiece mainPiece;
        [SerializeField] PiecesContainer piecesContainer;
        int total;
        int num;
        List<ItemData> items;
        int itemID;
        string lastIngredient;
        public override void OnInit()
        {
            int level = 0;

            if(GamesManager.Instance != null)
                level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;

            items = cookingData.GetItems(level);
            print("items: " + items.Count);
            menu.Init(items);
            mainPiece.Init();
            InitIngredient();
        }
        void InitIngredient()
        {
            num = 0;
            state = states.playing;
            total = items[itemID].num;
            string ingredient = items[itemID].item.ToString();
            mainPiece.InitIngredient("qty_" + ingredient, total);
            if(lastIngredient != "")  mainPiece.InitIngredient("qty_" + lastIngredient, 0); // RESET LAST
            lastIngredient = ingredient;
            piecesContainer.Initialize();
            piecesContainer.InitIngredient(this, items[itemID].item, total);
        }
        public void OnPieceAdded()
        {
            cookingData.PieceDone(itemID);
            total--; num++;

            numFeedback.Init(num);

            if (total <= 0)
               StartCoroutine( NextIngredient() );

          //  menu.Refresh(cookingData.items[itemID]);
        }
        IEnumerator NextIngredient()
        {
            YaguarLib.Events.Events.OnPlaySound(YaguarLib.Audio.AudioManager.types.REWARD);
            state = states.done;
            yield return new WaitForSeconds(2);
            itemID++;
            if (itemID >= items.Count)
                Next();
            else
            {
                InitIngredient();
            }
        }
        public bool CanMove()
        {
            return state == states.playing;
        }
    }
}
