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
        List<PlanetItemUI> photoItems;

        public void Init(List<PlanetData> allPlanets, List<PlanetName> itemsNames, System.Action<string,string> onClick)
        {
            allItems = new List<PlanetItemUI>();
            photoItems = new List<PlanetItemUI>();
            Utils.RemoveAllChildsIn(container);
            foreach(PlanetData c in allPlanets)
            {
                PlanetItemUI.PlanetState planetState = c.lastPhoto==null ? PlanetItemUI.PlanetState.undone : PlanetItemUI.PlanetState.done;
                if (itemsNames.Any(name => name == c.planetName))
                    planetState = PlanetItemUI.PlanetState.normal;
                PlanetItemUI ci = Instantiate(items_to_add, container);
                ci.Init(c, planetState, () => onClick(c.planetName.ToString(),"voices"));
                allItems.Add(ci);
                if (planetState == PlanetItemUI.PlanetState.normal)
                    photoItems.Add(ci);
            }
        }

        public void SetPlanetDone(PlanetName planetName) {
            PlanetItemUI pi = photoItems.Find(x => x.Planet_Name == planetName);
            if (pi != null)
                pi.SetDone();
        }

        public Vector3 GetItemPosition(PlanetName planetName) {
            PlanetItemUI pi = photoItems.Find(x => x.Planet_Name == planetName);
            return pi != null ? pi.transform.position : Vector3.zero;
        }

        public void OpenSlotDialog(System.Action<Vector3> onDone, PlanetName planetName, System.Action OnDone) {
            foreach(PlanetItemUI piui in photoItems) {
                piui.SetAsButton(planetName, () => {
                    UpdatePhotoItemsState();
                    OnDone();
                });
            }
        }
        void UpdatePhotoItemsState() {
            foreach (PlanetItemUI piui in photoItems) {
                piui.UpdatePlanetSate();
            }
        }
    }
}