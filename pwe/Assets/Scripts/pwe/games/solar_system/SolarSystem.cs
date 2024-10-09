using Pwe.Games.SolarSystem.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] PlanetsManager planetsManager;        
        [SerializeField] SolarSysMenuUI menu;
        int total;

        public override void OnInit() {
            SetPlanets();

            /*List<CookingItemData> items = cookingData.GetItems();
            menu.Init(items);
            total = items[0].num;*/
        }

        public void SetPlanets() {
            planetsManager.Init(planetsData);
            planetsManager.RemoveAllPlanets();
            foreach (PlanetData pd in planetsData.planets) {
                planetsManager.AddPlanet(pd);
            }
        }

        public void OnPieceAdded()
        {
            /*cookingData.PieceDone();
            int num = cookingData.items[0].num;

            numFeedback.Init(total - num);

            if (num < 1)
                Next();
            else
                menu.Refresh(cookingData.items[0]);*/
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SolarSystem))]
    class SolarSystemEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            SolarSystem solarSystem = (SolarSystem)target;
            //SolarSystem.SetPlanets = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
            if (GUILayout.Button("Reload Planets")) {
                solarSystem.SetPlanets();
            }
        }
    }
#endif
}
