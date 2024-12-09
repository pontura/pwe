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
        List<CookingItem> allItems;

        List<ItemData> items;
        CookingData cookingData;

        public void Init(List<ItemData> items, CookingData cookingData)
        {
            this.items = items;
            this.cookingData = cookingData; 
        }
        void OnEnable()
        {
            if(items != null)
                StartCoroutine(Anim(items, cookingData));
        }
        IEnumerator Anim(List<ItemData> items, CookingData cookingData)
        {
            allItems = new List<CookingItem>();
            Utils.RemoveAllChildsIn(container);
            foreach (ItemData c in items)
            {
                if (c.num > 0)
                {
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
        public void SetType(bool isCutscene)
        {
            foreach (GameObject go in cutsceneeGO)
                go.SetActive(false);
            foreach (GameObject go in ingameGO)
                go.SetActive(false);

            if (isCutscene)
            {
                foreach (GameObject go in cutsceneeGO)
                    go.SetActive(true);
            }
            else
            {
                foreach (GameObject go in ingameGO)
                    go.SetActive(true);
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