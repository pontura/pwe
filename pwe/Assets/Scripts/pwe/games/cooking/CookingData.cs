using System;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;
using static Pwe.Games.Cooking.IngredientsData;
using static Pwe.Games.Cooking.LevelsData;
namespace Pwe.Games.Cooking
{
    public class CookingData : MonoBehaviour
    {
        [SerializeField] LevelsData levelsData;
        [SerializeField] IngredientsData ingredientsData;

        string part = "pizza";
        public string Part { get { return part; } set { part = value; } }

        public Sprite GetIngredient(string ingredientName, int id = 0)
        {
            foreach (IngredientsData.IngredientData ing in ingredientsData.ingredients)
                if (ing.item.ToString() == ingredientName)
                    return ing.GetSprite(id);
            return null;
        }
        public Sprite GetBase(string n)
        {
            foreach (IngredientsData.BaseData b in ingredientsData.bases)
                if (b.baseName == n)
                    return b.asset;
            return null;
        }
        public List<IngredientData> GetIngredientsData() { return ingredientsData.ingredients; } 
        int num;
        public List<ItemData> GetItems(int level)
        {
            if (level >= levelsData.GetPart(Part).Count - 1)
                level = levelsData.GetPart(Part).Count - 1;

            LevelData levelData = levelsData.GetPart(Part)[level];
            List<ItemData> items = levelData.ingredients;
            num = items[0].num;  
            return items;
        }
        public LevelData GetLevelData(int level)
        {
            if (level >= levelsData.GetPart(Part).Count - 1)
                level = levelsData.GetPart(Part).Count - 1;

            return levelsData.GetPart(Part)[level];
        }
        public void PieceDone(int itemID)
        {
            num--;
            if (num <= 0)
                num = 0;
        }
    }

}