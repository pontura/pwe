using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using Pwe.Core;
using System.Collections;
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
        [SerializeField] PlanetListManager planetListManager;
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

        [SerializeField] ButtonProgressBar buttonProgressBar;

        bool _paused;

        CameraPan _cameraPan;

        int totalPlanets;
        List<PlanetName> planetsDone;

        public override void OnInitialize() {
            camClickInput.OnClickInput += OnClickInput;
            camClickInput.OnClickBlocked += OnClickBlocked;
            //planetsManager.OnPlanetClicked += levelsManager.OnPlanetClicked;
            //levelsManager.OnPlanetDone += menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone += SetPhotoDone;
            levelsManager.OnLevelCompleted += OnLevelCompleted;

            levelsManager.SetRiveTexture(Game.rive);

            screenshot.shotRes = new Vector2Int((int)(shotSizeScreenHeightFactor * Screen.height), (int)(shotSizeScreenHeightFactor * Screen.height));

            _cameraPan = GetComponent<CameraPan>();

            backButton.Init(Back);
            ingameAudio.PlayMusic("music");

            buttonProgressBar.Init(()=>levelCompletedPopup.Popup("LEVEL COMPLETED!", onContinue: Pwe.Core.GamesManager.Instance != null ? Core.Events.ExitGame : Back));
            buttonProgressBar.SetProgress(false);
            buttonProgressBar.SetInteraction(false);
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
            if (System.Enum.TryParse(reportedEvent.Name, out PlanetName planetName)) {
                if (planetName != PlanetName.none) {
                    delayedShot = true;
                }
                levelsManager.OnPlanetClicked(planetName);
            } else if (System.Enum.TryParse(reportedEvent.Name.Replace("list_", ""), out planetName)) {
                ingameAudio.Play(planetName.ToString(), AudioManager.channels.VOICES);
            } else if (System.Enum.TryParse(reportedEvent.Name.Replace("_path", ""), out planetName)) {
                Game.rive.SetNumber("game", reportedEvent.Name, Random.Range(0,3));
            } else {
                if (int.TryParse(reportedEvent.Name.Replace("trivia_btn_", ""), out int btnId))
                    triviaManager.OnButtonPressed(selectedPlanet, btnId);
                else
                    Debug.LogError("Error on parse trivia button index");
            }
        }

        private void OnDestroy() {
            camClickInput.OnClickInput -= OnClickInput;
            camClickInput.OnClickBlocked -= OnClickBlocked;
            //planetsManager.OnPlanetClicked -= levelsManager.OnPlanetClicked;
            //levelsManager.OnPlanetDone -= menuUI.SetPlanetDone;
            levelsManager.OnPlanetDone -= SetPhotoDone;
            levelsManager.OnLevelCompleted -= OnLevelCompleted;
        }

        void OnClickInput(Vector2 pos) {
            StartCoroutine(Takeshot(pos));
        }

        void OnClickBlocked() {
            levelsManager.OnPlanetClicked(PlanetName.none);
        }

        IEnumerator Takeshot(Vector2 pos) {
            //Debug.Log("#delayedShot: " + delayedShot);
            yield return new WaitForEndOfFrame();
            planetListManager.Show(false);
            yield return new WaitForEndOfFrame();
            if (delayedShot) {
                shotAction = () => Shot(pos);
                delayedShot = false;
            } else
                Shot(pos);
        }

        bool delayedShot;
        System.Action shotAction;
        void Shot(Vector2 pos) {
            //Debug.Log("% shot");
            if (!_paused) {
                Debug.Log("#Mouse Pos: " + pos.x + ", " + pos.y);                
                _paused = true;                
                camClickInput.SetShotSize(screenshot.shotRes);
                screenshot.TakeShot(pos, (tex) => OnCaptureDone(tex, pos));
                //planetsManager.Play(false);
                dinoFlash.SetActive(true);
            }
        }
        
        void OnCaptureDone(Texture2D tex, Vector2 pos) {
            //Debug.Log("#OnCaptureDone");
            photoUI.Init(tex, OnContinueMoving);
            /*photoUI.FadeSize(screenshot.shotRes, screenshot.shotRes, 0.2f);
            photoUI.FadePosition(pos, pos, 0.2f);
            photoUI.FadeAngle(Vector3.zero, Vector3.zero, 0.2f);*/
            photoUI.FadeSize(shotInitialSize, shotFinalSize, 0.2f);
            photoUI.FadePosition(pos, Vector2.Lerp(pos,new Vector2(Screen.width*0.5f, Screen.height*0.5f),0.25f), 0.2f);
            photoUI.FadeAngle(Vector3.zero, new Vector3(0,0,Random.Range(-15,15)), 0.2f);
            ingameAudio.Play("photo", AudioManager.channels.UI);
            planetListManager.Show(true);
        }

        void SetPhotoDone(PlanetName planetName) {
            //Debug.Log("#SetPhotoDone");
            if (planetName != PlanetName.none) {
                photoUI.SetDone(true);
                photoUI.SetDelayedFly(true);
                //photoUI.FlyTo(new Vector2((int)(Screen.width * 0.5f), 0));
                //photoUI.FlyTo(menuUI.GetItemPosition(planetName));
                Game.rive.SetBoolInArtboard(planetName.ToString(), "face", true);
                StartCoroutine(PhotoDone(planetName));
            } else {
                if (shotAction != null) {
                    shotAction();
                    shotAction = null;
                }
            }
        }

        IEnumerator PhotoDone(PlanetName planetName) {
            //Debug.Log("#PhotoDone");
            yield return new WaitForEndOfFrame();
            shotAction();
            shotAction = null;
            yield return new WaitForEndOfFrame();
            selectedPlanet = planetName;              
            
            
            //planetsManager.Play(false);
            
            

            //StartCoroutine(menuUI.OpenSlotDialog(planetName, OnSelectSlot));
            triviaManager.ShowTrivia(true);

            //planetsData.SavePlanetLastPhoto(planetName, screenshot.Texture);
            
            planetsData.SetPlanetLevelComplete(planetName);

            ingameAudio.Play(planetName.ToString(), AudioManager.channels.VOICES);
            ingameAudio.Play("click_right", AudioManager.channels.UI);
        }

        PlanetName selectedPlanet;
        void OnSelectSlot(PlanetName pressed) {
            if (pressed==selectedPlanet) {
                ingameAudio.Play("click_right", AudioManager.channels.UI);
                if (!planetsDone.Contains(selectedPlanet))
                    planetsDone.Add(selectedPlanet);
                Game.rive.SetBoolInArtboard(selectedPlanet.ToString(), "face", false);
                StartCoroutine(OnSelectSlotDone());
            } else {
                ingameAudio.Play("click_wrong", AudioManager.channels.UI);
            }
            ingameAudio.Play(pressed.ToString(), AudioManager.channels.VOICES);
        }



        System.Collections.IEnumerator OnSelectSlotDone() {
            yield return new WaitForSecondsRealtime(0.5f);
            //planetsManager.Play(true);
            triviaManager.ShowTrivia(false);
            photoUI.Close();
            //photoUI.Invoke(nameof(photoUI.Close), 0.5f);
            planetListManager.SetPlanetDone(selectedPlanet);
            buttonProgressBar.SetProgress(planetsDone.Count, totalPlanets);
            if (_levelCompleted) {
                List<UnityEngine.Color> colors = planetsData.planets.Where(e => planetsDone.Any(x=>e.planetName==x)).Select(item => item.color).ToList();
                Events.OnWinParticles(colors, new Vector2());
                buttonProgressBar.SetInteraction(true);
                _cameraPan.Panning = false;
                _levelCompleted = false;
                //levelCompletedPopup.Popup("LEVEL COMPLETED!", delay: photoUI.CloseDelay + photoUI.FlyDelay + menuUI.Menu2Delay, onContinue: Pwe.Core.GamesManager.Instance != null ? Core.Events.ExitGame : Back);

                //levelCompletedPopup.Popup("LEVEL COMPLETED!",onContinue: Pwe.Core.GamesManager.Instance != null ? Core.Events.ExitGame : Back);
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
            //menuUI.Init(planetsData.planets, levelPlanetNames, ingameAudio.Play);
            //photoUI.FlyOnWrong(new Vector2(Screen.width, Screen.height));
            //photoUI.FlyTo(new Vector2(Screen.width, Screen.height));

            planetsDone = new();
            totalPlanets = levelPlanetNames.Count;

            triviaManager.Init(Game.rive, levelPlanetNames,OnSelectSlot);
            planetListManager.Init(Game.rive, levelPlanetNames);
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
