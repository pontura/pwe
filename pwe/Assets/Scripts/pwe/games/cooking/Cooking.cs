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

        public override void OnInit()
        {
            List<CookingItemData> items = cookingData.GetItems();
            menu.Init(items);
            total = items[0].num;
            Delayed();
        }
        void Delayed()
        {
            mainPiece.Init("init_olives");
        }
        
        public void OnPieceAdded()
        {
            cookingData.PieceDone();
            int num = cookingData.items[0].num;

            numFeedback.Init(total - num);

            if (num < 1)
                Next();
            else
                menu.Refresh(cookingData.items[0]);
        }
    }
}
