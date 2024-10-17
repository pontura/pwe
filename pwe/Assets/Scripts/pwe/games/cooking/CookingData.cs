using System;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Games.Cooking
{
    public class CookingData : MonoBehaviour
    {
        public List<CookingItemData> items;

        public enum Items
        {
            tomatoes,
            olives
        }
        [Serializable] public class CookingItemData
        {
            public Items item;
            public int num;
        }
        public List<CookingItemData> GetItems()
        {
            return items;
        }
        public void PieceDone(int itemID)
        {
            items[itemID].num--;
            if (items[itemID].num <= 0)
                items[itemID].num = 0;
        }
    }

}