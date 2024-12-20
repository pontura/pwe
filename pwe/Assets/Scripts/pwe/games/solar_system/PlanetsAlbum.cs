using YaguarLib.UI;
using UnityEngine;
using Pwe.Games.SolarSystem.UI;
using System.Collections;
using System.Collections.Generic;
using YaguarLib.Audio;
using Rive;
using System.Linq;

namespace Pwe.Games.SolarSystem
{
    public class PlanetsAlbum : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] ButtonUI playButton;
        [SerializeField] List<PlanetItemUI> albumItems;
        [SerializeField] IngameAudio ingameVoiceOvers;
        [SerializeField] LevelsManager levelsManager;

        public override void OnInitialize() {
            playButton.Init(Skip);
        }
        public override void OnInit() {
            base.OnInit();

            GetRiveTexture().OnRiveEvent += RiveScreen_OnRiveEvent;
            GetRiveTexture().ActivateArtboard("summary");

            List<PlanetName> levelPlanets = levelsManager.GetCurrentLevelPlanets().OrderByDescending(x => (int)x).ToList();

            Debug.Log("% Album: "+ levelPlanets[0]);

            foreach (PlanetData pd in planetsData.planets) {
                if ((int)pd.planetName <= (int)levelPlanets[0]) {
                    if (pd.hasNewPhoto)
                        Game.rive.SetTriggerInArtboard(pd.planetName.ToString(), "popup");
                    if (pd.levelCompleted)
                        Game.rive.SetBoolInArtboard(pd.planetName.ToString(), "face", true);
                } else {
                    Game.rive.SetBoolInArtboard(pd.planetName.ToString(), "inactive", true);
                }

            }
            //SetAlbumItems();
        }

        public override void OnHide() {
            GetRiveTexture().OnRiveEvent -= RiveScreen_OnRiveEvent;
        }

        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent) {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            if (System.Enum.TryParse(reportedEvent.Name, out PlanetName planetName)) {
                ingameVoiceOvers.Play(planetName.ToString(), AudioManager.channels.VOICES);
            } else {
                Debug.LogError("Error on parse trivia button index");
            }
        }

        void SetAlbumItems() {
            foreach(PlanetItemUI piui in albumItems) {
                PlanetData pd = planetsData.planets.Find(x => x.planetName == piui.Planet_Name);
                if (pd != null) {
                    piui.SetImage(pd);
                } 
               piui.SetButton(() => ingameVoiceOvers.Play(piui.Planet_Name.ToString(), AudioManager.channels.VOICES));
            }
        }

        void Skip() {
            Next();
        }
    }
}
