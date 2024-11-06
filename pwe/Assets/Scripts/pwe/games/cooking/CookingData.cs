using System;
using System.Collections.Generic;
using UnityEngine;
using static Pwe.Games.Cooking.LevelsData;
namespace Pwe.Games.Cooking
{
    public class CookingData : MonoBehaviour
    {
        [SerializeField] LevelsData levelsData;
        int num;

        public List<ItemData> GetItems()
        {
            LevelData levelData = levelsData.levels[0];
            List<ItemData> items = levelData.ingredients;
            num = items[num].num;  
            return items;
        }
        public void PieceDone(int itemID)
        {
            num--;
            if (num <= 0)
                num = 0;
        }
    }

}