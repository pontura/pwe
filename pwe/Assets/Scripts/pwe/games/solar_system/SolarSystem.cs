using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YaguarLib.Xtras;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] PlanetsManager planetsManager;        
        [SerializeField] SolarSysMenuUI menu;
        [SerializeField] CamClickInput camClickInput;
        [SerializeField] Screenshot screenshot;

        [SerializeField] PhotoUI photoUI;
        [SerializeField] GameObject dinoFlash;

        bool _paused;

        public override void OnInit() {
            camClickInput.OnClickInput += Takeshot;
            InitPlanets();

            /*List<CookingItemData> items = cookingData.GetItems();
            menu.Init(items);
            total = items[0].num;*/
        }

        private void OnDestroy() {
            camClickInput.OnClickInput -= Takeshot;
        }

        void Takeshot(Vector2 pos) {
            if (!_paused) {
                _paused = true;
                screenshot.TakeShot(pos, OnCaptureDone);
                planetsManager.Play(false);
                dinoFlash.SetActive(true);
            }
        }

        void OnCaptureDone(Texture2D tex) {
            photoUI.Init(tex, OnContinueMoving);            
        }

        void OnContinueMoving() {
            _paused = false;
            planetsManager.Play(true);
            dinoFlash.SetActive(false);
        }

        public void InitPlanets() {
            planetsManager.RemoveAllPlanets();
            planetsManager.Init(planetsData);            
            /*foreach (PlanetData pd in planetsData.planets) {
                planetsManager.AddPlanet(pd);
            }*/
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
                solarSystem.InitPlanets();
            }
        }
    }
#endif
}
