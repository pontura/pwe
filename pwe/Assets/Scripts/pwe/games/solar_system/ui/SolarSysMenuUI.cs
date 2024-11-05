using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;
using static Pwe.Games.SolarSystem.PlanetsData;
using System.Linq;

namespace Pwe.Games.SolarSystem.UI
{
    public class SolarSysMenuUI : MonoBehaviour
    {
        [SerializeField] PlanetItemUI items_to_add;
        [SerializeField] Transform container;
        List<PlanetItemUI> allItems;

        public void Init(List<PlanetData> allPlanets, List<PlanetName> itemsNames, System.Action<string,string> onClick)
        {
            allItems = new List<PlanetItemUI>();
            Utils.RemoveAllChildsIn(container);
            foreach(PlanetData c in allPlanets)
            {
                PlanetItemUI.PlanetState planetState = c.lastPhoto==null ? PlanetItemUI.PlanetState.undone : PlanetItemUI.PlanetState.done;
                if (itemsNames.Any(name => name == c.planetName))
                    planetState = PlanetItemUI.PlanetState.photo;
                PlanetItemUI ci = Instantiate(items_to_add, container);
                ci.Init(c, planetState, () => onClick(c.planetName.ToString(),"voices"));
                allItems.Add(ci);
            }
        }

        public void SetPlanetDone(PlanetName planetName) {
            PlanetItemUI pi = allItems.Find(x => x.Planet_Name == planetName);
            if (pi != null)
                pi.SetDone();
        }

        public Vector3 GetItemPosition(PlanetName planetName) {
            PlanetItemUI pi = allItems.Find(x => x.Planet_Name == planetName);
            return pi != null ? pi.transform.position : Vector3.zero;
        }
    }
}