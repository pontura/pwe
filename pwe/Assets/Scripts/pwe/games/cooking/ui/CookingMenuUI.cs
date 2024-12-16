using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingMenuUI : MonoBehaviour
    {
        [SerializeField] CookingItem items_to_add;
        [SerializeField] GameObject[] ingameGO;
        [SerializeField] GameObject[] cutsceneeGO;
        [SerializeField] Transform container;
        [SerializeField] GameObject paper;
        List<CookingItem> allItems;

        List<ItemData> items;
        CookingData cookingData;

        public void Init(List<ItemData> items, CookingData cookingData)
        {
            this.items = items;
            this.cookingData = cookingData;
            print("init" + items.Count);
            StartCoroutine(Anim(items, cookingData));
        }
        IEnumerator Anim(List<ItemData> items, CookingData cookingData)
        {
            allItems = new List<CookingItem>();
            if(paper != null) 
                paper.transform.SetParent(transform);

            Utils.RemoveAllChildsIn(container);


            if (paper != null)
            {
                paper.transform.SetParent(container);
            }
            foreach (ItemData c in items)
            {
                if (c.num > 0)
                {
                    print("c " + c.item.ToString() + " num " + c.num);
                    yield return new WaitForSeconds(0.5f);
                    CookingItem ci = Instantiate(items_to_add, container);
                    Sprite sprite = cookingData.GetIngredient(c.item.ToString());
                    ci.Init(c, sprite);
                    allItems.Add(ci);
                }
            }
        }
        public void OnIngredientDone(string s)
        {
            foreach (CookingItem c in allItems)
            {
                if (c.data.item.ToString() == s && !c.done)
                    c.OnReady();
            }
        }
        public void Refresh(ItemData itemData)
        {
            //CookingItem c = GetItem(itemData);
            //c.Init(itemData);
        }
        CookingItem GetItem(ItemData itemData)
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