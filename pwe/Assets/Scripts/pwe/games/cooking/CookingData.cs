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

        public List<ItemData> GetItems(int level)
        {
            if (level >= levelsData.levels.Count - 1)
                level = levelsData.levels.Count - 1;

            Debug.Log("Cooking Get Items level: " + level);

            LevelData levelData = levelsData.levels[level];
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