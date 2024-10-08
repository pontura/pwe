using Pwe.Games.SolarSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] SolarSysMenuUI menu;
        int total;

        public override void OnInit()
        {
            /*List<CookingItemData> items = cookingData.GetItems();
            menu.Init(items);
            total = items[0].num;*/
        }
        public void OnPieceAdded()
        {
            /*cookingData.PieceDone();
            int num = cookingData.items[0].num;

            numFeedback.Init(total - num);

            if (num < 1)
                Next();
            else
                menu.Refresh(cookingData.items[0]);*/
        }
    }
}
