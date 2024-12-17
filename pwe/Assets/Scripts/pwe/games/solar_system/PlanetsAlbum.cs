using YaguarLib.UI;
using UnityEngine;
using Pwe.Games.SolarSystem.UI;
using System.Collections;
using System.Collections.Generic;
using YaguarLib.Audio;


namespace Pwe.Games.SolarSystem
{
    public class PlanetsAlbum : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] ButtonUI playButton;
        [SerializeField] List<PlanetItemUI> albumItems;
        [SerializeField] IngameAudio ingameVoiceOvers;

        public override void OnInitialize() {
            playButton.Init(Skip);
        }
        public override void OnInit() {
            base.OnInit();
            SetAlbumItems();
        }

        void SetAlbumItems() {
            foreach(PlanetItemUI piui in albumItems) {
                PlanetData pd = planetsData.planets.Find(x => x.planetName == piui.Planet_Name);
                if (pd != null) {
                    piui.SetImage(pd);
                } 
              // piui.SetButton(() => ingameVoiceOvers.Play(piui.Planet_Name.ToString(), AudioManager.channels.VOICES));
            }
        }

        void Skip() {
            Next();
        }
    }
}
