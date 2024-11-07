using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YaguarLib.Xtras;
using YaguarLib.UI;
using System.Linq;
using YaguarLib.Audio;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] LevelsManager levelsManager;
        [SerializeField] PlanetsManager planetsManager;        
        [SerializeField] SolarSysMenuUI menuUI;
        [SerializeField] ButtonUI backButton;
        [SerializeField] IngamePopup levelCompletedPopup;
        [SerializeField] CamClickInput camClickInput;
        [SerializeField] Screenshot screenshot;

        [SerializeField] IngameAudio ingameVoiceOvers;
        [SerializeField] PhotoUI photoUI;
        [SerializeField] GameObject dinoFlash;

        [SerializeField] float shotSizeScreenHeightFactor;

        [SerializeField] Vector2 shotInitialSize, shotFinalSize;

        bool _paused;

        CameraPan _cameraPan;

        public override void OnInitialize() {
            camClickInput.OnClickInput += Takeshot;
            planetsManager.OnPlanetClicked += levelsManager.OnPlanetClicked;
            levelsManager.OnPlanetDone += menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone += SetPhotoDone;
            levelsManager.OnLevelCompleted += OnLevelCompleted;

            screenshot.shotRes = new Vector2Int((int)(shotSizeScreenHeightFactor * Screen.height), (int)(shotSizeScreenHeightFactor * Screen.height));

            _cameraPan = GetComponent<CameraPan>();

            backButton.Init(Back);
        }

        public override void OnInit() {
            base.OnInit();

            InitPlanets();

            _cameraPan.Panning = true;
        }

        private void OnDestroy() {
            camClickInput.OnClickInput -= Takeshot;
            planetsManager.OnPlanetClicked -= levelsManager.OnPlanetClicked;
            levelsManager.OnPlanetDone -= menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone -= SetPhotoDone;
            levelsManager.OnLevelCompleted -= OnLevelCompleted;
        }

        void Takeshot(Vector2 pos) {
            if (!_paused) {
                //Debug.Log("#Mouse Pos: " + pos.x + ", " + pos.y);
                _paused = true;
                camClickInput.SetShotSize(screenshot.shotRes);
                screenshot.TakeShot(pos, (tex) => OnCaptureDone(tex,pos));
                //planetsManager.Play(false);
                dinoFlash.SetActive(true);
            }
        }

        void OnCaptureDone(Texture2D tex, Vector2 pos) {
            Debug.Log("#OnCaptureDone");
            photoUI.Init(tex, OnContinueMoving);
            photoUI.FadeSize(shotInitialSize, shotFinalSize, 0.2f);
            photoUI.FadePosition(pos, Vector2.Lerp(pos,new Vector2(Screen.width*0.5f, Screen.height*0.5f),0.25f), 0.2f);
            photoUI.FadeAngle(Vector3.zero, new Vector3(0,0,Random.Range(-15,15)), 0.2f);
            //photoUI.FlyTo(new Vector2(Screen.width, Screen.height));
        }

        void SetPhotoDone(PlanetName planetName) {
            Debug.Log("#SetPhotoDone");
            if (planetName != PlanetName.none) {
                photoUI.SetDone(true);
                photoUI.SetDelayedFly(true);
                planetsManager.Play(false);
                photoUI.FlyTo(menuUI.GetItemPosition(planetName));
                StartCoroutine(menuUI.OpenSlotDialog(photoUI.FlyTo, planetName, OnSelectSlotDone));
                planetsData.SavePlanetLastPhoto(planetName, screenshot.Texture);
                ingameVoiceOvers.Play(planetName.ToString(), "voices");
            }
        }

        void OnSelectSlotDone() {
            planetsManager.Play(true);
            photoUI.Invoke(nameof(photoUI.Fly), 1);
            if (_levelCompleted) {
                _cameraPan.Panning = false;
                levelCompletedPopup.Popup("LEVEL COMPLETED!", delay: photoUI.CloseDelay + photoUI.FlyDelay + menuUI.Menu2Delay, onContinue: Back);
            }
        }

        void OnContinueMoving() {
            _paused = false;
            //planetsManager.Play(true);
            dinoFlash.SetActive(false);
        }

        bool _levelCompleted;
        void OnLevelCompleted() {
            Debug.Log("#OnLevelCompleted");
            _levelCompleted = true;
            //levelCompletedPopup.Popup("LEVEL COMPLETED!", delay: photoUI.CloseDelay+photoUI.FlyDelay, onContinue:Back);
        }

        public void InitPlanets() {
            _levelCompleted = false;
            planetsManager.RemoveAllPlanets();
            SpaceData sd = levelsManager.InitLevel();
            planetsManager.Init(planetsData, sd);
            List<PlanetName> levelPlanetNanes = sd.LevelItems.Select(item => item.planetName).ToList();
            //IEnumerable<PlanetData> levelPlanetsData = planetsData.planets.Where(item => sd.LevelItems.Any(category => category.planetName == item.planetName));
            menuUI.Init(planetsData.planets, levelPlanetNanes, ingameVoiceOvers.Play);
            photoUI.FlyOnWrong(new Vector2(Screen.width, Screen.height));
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
