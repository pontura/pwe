using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pwe.Core;
using Pwe.Games.Common;

namespace Pwe.Games.SolarSystem
{
    public class LevelsManager : Pwe.Games.Common.LevelsManager
    {

        [SerializeField] private List<PlanetName> clickedPlanets;

        [SerializeField] private List<PlanetName> levelPlanets;

        public event System.Action<PlanetName> OnPlanetDone;

        public override event System.Action OnLevelCompleted;

        void Awake() {
            clickedPlanets = new List<PlanetName>();
            SetCurrentLevelIndex();
        }

        protected override void SetCurrentLevelIndex() {
            base.SetCurrentLevelIndex();

            //CurrentLevelIndex = PlayerPrefs.GetInt("solar_system_level", 0);

            if (CurrentLevelIndex >= levels.Count) {
                CurrentLevelIndex = 0;
                GamesManager.Instance.All[(int)(int)gameType].level = 0;
            }
        }

        public override LevelData InitLevel() {
            levelPlanets = new();
            foreach(SpaceData.LevelItem li in (levels[CurrentLevelIndex] as SpaceData).LevelItems ) {
                if(li.planetName!=PlanetName.none)
                    levelPlanets.Add(li.planetName);
            }
            return levels[CurrentLevelIndex];
        }

        public void OnPlanetClicked(PlanetName planetName) {
            clickedPlanets.Add(planetName);
            StartCoroutine(CheckLevelDone());
        }

        IEnumerator CheckLevelDone() {
            yield return new WaitForEndOfFrame();
            if (clickedPlanets.Count == 1) {
                if (OnPlanetDone != null) {
                    Debug.Log("# CheckLevelDone: Done");
                    OnPlanetDone(clickedPlanets[0]);
                    if (levelPlanets.Count > 0) {
                        levelPlanets.Remove(clickedPlanets[0]);
                        if (levelPlanets.Count == 0 && OnLevelCompleted != null) {
                            CurrentLevelIndex++;
                            if(GamesManager.Instance!=null)
                                GamesManager.Instance.All[(int)GameData.GAMES.PHOTOS].LevelUp();
                            if (CurrentLevelIndex >= levels.Count)
                                CurrentLevelIndex = 0;
                            PlayerPrefs.SetInt("solar_system_level", CurrentLevelIndex);
                            OnLevelCompleted();
                        }
                    }
                }                
            }
            yield return new WaitForEndOfFrame();
            clickedPlanets = new List<PlanetName>();
        }
    }
}