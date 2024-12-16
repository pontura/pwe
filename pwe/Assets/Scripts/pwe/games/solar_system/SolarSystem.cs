using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YaguarLib.Xtras;
using YaguarLib.UI;
using System.Linq;
using YaguarLib.Audio;
using Rive;

namespace Pwe.Games.SolarSystem
{
    public class SolarSystem : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] LevelsManager levelsManager;
        [SerializeField] PlanetsManager planetsManager;
        [SerializeField] TriviaManager triviaManager;
        [SerializeField] SolarSysMenuUI menuUI;
        [SerializeField] ButtonUI backButton;
        [SerializeField] IngamePopup levelCompletedPopup;
        [SerializeField] CamClickInput camClickInput;
        [SerializeField] Screenshot screenshot;

        [SerializeField] IngameAudio ingameAudio;
        [SerializeField] PhotoUI photoUI;
        [SerializeField] GameObject dinoFlash;

        [SerializeField] float shotSizeScreenHeightFactor;

        [SerializeField] Vector2 shotInitialSize, shotFinalSize;

        bool _paused;

        CameraPan _cameraPan;

        public override void OnInitialize() {
            camClickInput.OnClickInput += Takeshot;
            //planetsManager.OnPlanetClicked += levelsManager.OnPlanetClicked;
            //levelsManager.OnPlanetDone += menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone += SetPhotoDone;
            levelsManager.OnLevelCompleted += OnLevelCompleted;

            levelsManager.SetRiveTexture(Game.rive);

            screenshot.shotRes = new Vector2Int((int)(shotSizeScreenHeightFactor * Screen.height), (int)(shotSizeScreenHeightFactor * Screen.height));

            _cameraPan = GetComponent<CameraPan>();

            backButton.Init(Back);
        }

        public override void OnInit() {
            base.OnInit();

            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;
            GetRiveTexture().ActivateArtboard("game");

            InitPlanets();

            _cameraPan.Panning = true;
        }

        public override void OnHide() {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent) {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            PlanetName planetName;
            if (System.Enum.TryParse(reportedEvent.Name, out planetName)) {
                levelsManager.OnPlanetClicked(planetName);
            //} else if (System.Enum.TryParse(reportedEvent.Name.Replace("trivia_btn_", ""), out planetName)) {
            } else {
                if (int.TryParse(reportedEvent.Name.Replace("trivia_btn_", ""), out int btnId))
                    triviaManager.OnButtonPressed(selectedPlanet, btnId);
                else
                    Debug.LogError("Error on parse trivia button index");
            }
        }

        private void OnDestroy() {
            camClickInput.OnClickInput -= Takeshot;
            //planetsManager.OnPlanetClicked -= levelsManager.OnPlanetClicked;
            //levelsManager.OnPlanetDone -= menuUI.SetPlanetDone;
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
            ingameAudio.Play("photo", "ui");
            photoUI.FlyTo(new Vector2(Screen.width, Screen.height));
        }

        void SetPhotoDone(PlanetName planetName) {
            Debug.Log("#SetPhotoDone");
            if (planetName != PlanetName.none) {
                selectedPlanet = planetName;
                
                photoUI.SetDone(true);
                photoUI.SetDelayedFly(true);
                planetsManager.Play(false);
                photoUI.FlyTo(menuUI.GetItemPosition(planetName));

                //StartCoroutine(menuUI.OpenSlotDialog(planetName, OnSelectSlot));
                triviaManager.ShowTrivia(true);

                //planetsData.SavePlanetLastPhoto(planetName, screenshot.Texture);

                ingameAudio.Play(planetName.ToString(), "voices");
                ingameAudio.Play("click_right", "ui");
            }
        }

        PlanetName selectedPlanet;
        void OnSelectSlot(PlanetName pressed) {
            if (pressed==selectedPlanet) {
                ingameAudio.Play("click_right", "ui");
                StartCoroutine(OnSelectSlotDone());
            } else {
                ingameAudio.Play("click_wrong", "ui");
            }                    
        }

        

        System.Collections.IEnumerator OnSelectSlotDone() {
            yield return new WaitForSecondsRealtime(0.5f);
            planetsManager.Play(true);
            triviaManager.ShowTrivia(false);
            photoUI.Invoke(nameof(photoUI.Fly), 1);
            if (_levelCompleted) {
                _cameraPan.Panning = false;
                levelCompletedPopup.Popup("LEVEL COMPLETED!", delay: photoUI.CloseDelay + photoUI.FlyDelay + menuUI.Menu2Delay, onContinue: Pwe.Core.GamesManager.Instance != null ? Core.Events.ExitGame : Back);
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
            SpaceData sd = levelsManager.InitLevel() as SpaceData;

            //planetsManager.Init(planetsData, sd, _cameraPan.cam.transform);

            List<PlanetName> levelPlanetNames = sd.LevelItems.Where(e=>e.planetName!=PlanetName.none).Select(item => item.planetName).ToList();
            //IEnumerable<PlanetData> levelPlanetsData = planetsData.planets.Where(item => sd.LevelItems.Any(category => category.planetName == item.planetName));
            menuUI.Init(planetsData.planets, levelPlanetNames, ingameAudio.Play);
            photoUI.FlyOnWrong(new Vector2(Screen.width, Screen.height));

            triviaManager.Init(Game.rive, levelPlanetNames,OnSelectSlot);
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
