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
        List<CookingItemData> items;
        int itemID;
        public override void OnInit()
        {
            items = cookingData.GetItems();
            menu.Init(items);
            mainPiece.Init();
            InitIngredient();
        }
        void InitIngredient()
        {
            state = states.playing;
            total = items[itemID].num;
            string ingredient = items[itemID].item.ToString();
            mainPiece.InitIngredient("init_" + ingredient);
            piecesContainer.Initialize();
            piecesContainer.InitIngredient(this, items[itemID].item, total);
        }
        public void OnPieceAdded()
        {
            cookingData.PieceDone(itemID);
            int num = cookingData.items[itemID].num;

            numFeedback.Init(total - num);

            if (num < 1)
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
