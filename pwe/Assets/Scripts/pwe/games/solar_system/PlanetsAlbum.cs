using YaguarLib.UI;
using UnityEngine;
using Pwe.Games.SolarSystem.UI;
using System.Collections;
using System.Collections.Generic;


namespace Pwe.Games.SolarSystem
{
    public class PlanetsAlbum : GameMain
    {
        [SerializeField] PlanetsData planetsData;
        [SerializeField] ButtonUI playButton;
        [SerializeField] List<PlanetItemUI> albumItems;
        
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
                    piui.SetImage(pd.lastPhoto);
                }
            }
        }

        void Skip() {
            Next();
        }
    }
}
