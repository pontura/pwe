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

        public void Init(IEnumerable<PlanetData> items)
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

        public void SetPlanetDone(PlanetName planetName) {
            PlanetItem pi = allItems.Find(x => x.Planet_Name == planetName);
            if (pi != null)
                pi.SetDone();
        }

        public Vector3 GetItemPosition(PlanetName planetName) {
            PlanetItem pi = allItems.Find(x => x.Planet_Name == planetName);
            return pi != null ? pi.transform.position : Vector3.zero;
        }
    }
}