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
            Debug.Log("num: " + num);
            Debug.Log("levelData: " + levelData);
            Debug.Log("ingredients: " + levelData.ingredients);
            Debug.Log("ingredients Count: " + levelData.ingredients.Count);
            List<ItemData> items = levelData.ingredients;
            num = items[0].num;  
            return items;
        }
        public LevelData GetLevelData(int level)
        {
            if (level >= levelsData.levels.Count - 1)
                level = levelsData.levels.Count - 1;

            Debug.Log("GetLevelData: " + level);
            return levelsData.levels[level];
        }
        public void PieceDone(int itemID)
        {
            num--;
            if (num <= 0)
                num = 0;
        }
    }

}