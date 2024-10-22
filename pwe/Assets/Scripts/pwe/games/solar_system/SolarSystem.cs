using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YaguarLib.Xtras;
using System.Linq;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] LevelsManager levelsManager;
        [SerializeField] PlanetsManager planetsManager;        
        [SerializeField] SolarSysMenuUI menuUI;
        [SerializeField] CamClickInput camClickInput;
        [SerializeField] Screenshot screenshot;

        [SerializeField] PhotoUI photoUI;
        [SerializeField] GameObject dinoFlash;


        [SerializeField] Vector2 shotInitialSize, shotFinalSize;

        bool _paused;

        public override void OnInit() {
            camClickInput.OnClickInput += Takeshot;
            planetsManager.OnPlanetClicked += levelsManager.OnPlanetClicked;
            levelsManager.OnPlanetDone += menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone += SetPhotoDone;

            InitPlanets();

            /*List<CookingItemData> items = cookingData.GetItems();
            menu.Init(items);
            total = items[0].num;*/
        }

        private void OnDestroy() {
            camClickInput.OnClickInput -= Takeshot;
            planetsManager.OnPlanetClicked -= levelsManager.OnPlanetClicked;
            levelsManager.OnPlanetDone -= menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone -= SetPhotoDone;
        }

        void Takeshot(Vector2 pos) {
            if (!_paused) {
                _paused = true;
                camClickInput.SetShotSize(screenshot.shotRes);
                screenshot.TakeShot(pos, (tex) => OnCaptureDone(tex,pos));
                //planetsManager.Play(false);
                dinoFlash.SetActive(true);
            }
        }

        void OnCaptureDone(Texture2D tex, Vector2 pos) {
            photoUI.Init(tex, OnContinueMoving);
            photoUI.FadeSize(shotInitialSize, shotFinalSize, 0.2f);
            photoUI.FadePosition(pos, Vector2.Lerp(pos,new Vector2(Screen.width*0.5f, Screen.height*0.5f),0.25f), 0.2f);
            photoUI.FadeAngle(Vector3.zero, new Vector3(0,0,Random.Range(-15,15)), 0.2f);
        }

        void SetPhotoDone(PlanetName planetName) {
            photoUI.SetDone(planetName != PlanetName.none);
        }

        void OnContinueMoving() {
            _paused = false;
            //planetsManager.Play(true);
            dinoFlash.SetActive(false);
        }

        public void InitPlanets() {
            planetsManager.RemoveAllPlanets();
            SpaceData sd = levelsManager.GetCurrentLevel();
            planetsManager.Init(planetsData, sd);
            IEnumerable<PlanetData> levelPlanetsData = planetsData.planets.Where(item => sd.LevelItems.Any(category => category.planetName == item.planetName));
            menuUI.Init(levelPlanetsData);       
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
