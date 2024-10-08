using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    public class SolarSysMenuUI : MonoBehaviour
    {
        [SerializeField] PlanetItem items_to_add;
        [SerializeField] Transform container;
        List<PlanetItem> allItems;

        public void Init(List<PlanetData> items)
        {
            allItems = new List<PlanetItem>();
            Utils.RemoveAllChildsIn(container);
            foreach(PlanetData c in items)
            {
                PlanetItem ci = Instantiate(items_to_add, container);
                ci.Init(c);
                allItems.Add(ci);
            }
        }
        public void Refresh(PlanetData itemData)
        {
            PlanetItem c = GetItem(itemData);
            c.Init(itemData);
        }
        PlanetItem GetItem(PlanetData itemData)
        {
            foreach (PlanetItem c in allItems)
            {
                if (c.data == itemData)
                    return c;
            }
            return null;
        }
    }
}