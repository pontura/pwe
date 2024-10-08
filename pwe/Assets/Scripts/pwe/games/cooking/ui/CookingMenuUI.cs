using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingMenuUI : MonoBehaviour
    {
        [SerializeField] CookingItem items_to_add;
        [SerializeField] Transform container;
        List<CookingItem> allItems;

        public void Init(List<CookingItemData> items)
        {
            allItems = new List<CookingItem>();
            Utils.RemoveAllChildsIn(container);
            foreach(CookingItemData c in items)
            {
                CookingItem ci = Instantiate(items_to_add, container);
                ci.Init(c);
                allItems.Add(ci);
            }
        }
        public void Refresh(CookingItemData itemData)
        {
            CookingItem c = GetItem(itemData);
            c.Init(itemData);
        }
        CookingItem GetItem(CookingItemData itemData)
        {
            foreach (CookingItem c in allItems)
            {
                if (c.data == itemData)
                    return c;
            }
            return null;
        }
    }
}