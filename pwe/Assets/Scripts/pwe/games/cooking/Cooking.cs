using Pwe.Games.Cooking.UI;
using System.Collections.Generic;
using UnityEngine;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking
{
    public class Cooking : GameMain
    {
        [SerializeField] CookingData cookingData;
        [SerializeField] CookingMenuUI menu;
        [SerializeField] NumFeedback numFeedback;
        [SerializeField] CookingMainPiece mainPiece;
        int total;
        List<CookingItemData> items;
        int itemID;
        public override void OnInit()
        {
            items = cookingData.GetItems();
            menu.Init(items);
            InitIngredient();
        }
        void InitIngredient()
        {
            total = items[itemID].num;
            mainPiece.Init("init_" + items[itemID].item.ToString());
        }
        public void OnPieceAdded()
        {
            cookingData.PieceDone();
            int num = cookingData.items[itemID].num;

            numFeedback.Init(total - num);

            if (num < 1)
                NextIngredient();
            else
                menu.Refresh(cookingData.items[itemID]);
        }
        void NextIngredient()
        {
            itemID++;
            if (itemID >= items.Count)
                Next();
            else
            {
                InitIngredient();
            }
        }
    }
}
