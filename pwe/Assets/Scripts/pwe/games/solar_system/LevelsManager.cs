using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pwe.Core;
using Pwe.Games.Common;
using System.Linq;

namespace Pwe.Games.SolarSystem
{
    public class LevelsManager : Pwe.Games.Common.LevelsManager {

        [SerializeField] private List<PlanetName> clickedPlanets;

        [SerializeField] private List<PlanetName> levelPlanets;

        public event System.Action<PlanetName> OnPlanetDone;

        public override event System.Action OnLevelCompleted;

        RiveTexture _riveTexture;

        float planetDelayRange = 2; // 2 seconds delay max        

        List<int> obstacles = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8 };

        void Awake() {
            clickedPlanets = new List<PlanetName>();
            SetCurrentLevelIndex();            
        }

        public void SetRiveTexture(RiveTexture rt) {
            _riveTexture = rt;
        }       

        protected override void SetCurrentLevelIndex() {
            base.SetCurrentLevelIndex();

            //CurrentLevelIndex = PlayerPrefs.GetInt("solar_system_level", 0);

            if (CurrentLevelIndex >= levels.Count) {
                CurrentLevelIndex = 0;
                GamesManager.Instance.All[(int)(int)gameType].LevelMain.level = 0;//
            }
        }

        public List<PlanetName> GetCurrentLevelPlanets() {
            return (levels[CurrentLevelIndex] as SpaceData).LevelItems.Where(e => e.planetName != PlanetName.none).Select(item => item.planetName).ToList();
        }

        public override LevelData InitLevel() {
            levelPlanets = new();
            foreach (PlanetName pn in System.Enum.GetValues(typeof(PlanetName))) {
                _riveTexture.SetBool("game", pn.ToString(), false);
            }
            
            foreach (SpaceData.LevelItem li in (levels[CurrentLevelIndex] as SpaceData).LevelItems) {
                if (li.planetName != PlanetName.none) {
                    _riveTexture.SetBool("game", li.planetName.ToString(), true);
                    _riveTexture.SetBoolInArtboard(li.planetName.ToString(), "spin", true);
                    levelPlanets.Add(li.planetName);
                    StartCoroutine(SetPlanetInitialSpeed(li.planetName));
                } else {
                    int id = obstacles[Random.Range(0, obstacles.Count)];
                    _riveTexture.SetBool("game", "obstacle_" + id, true);
                    obstacles.Remove(id);
                }
            }           
            
            return levels[CurrentLevelIndex];
        }

        public void PlanetsMove(bool move) {
            foreach (PlanetName pn in levelPlanets) {
                _riveTexture.SetNumber("game", pn.ToString() + "_speed", move ? 1 : 0);
            }
        }

        IEnumerator SetPlanetInitialSpeed(PlanetName pn) {
            yield return new WaitForSecondsRealtime(Random.value * planetDelayRange);
            _riveTexture.SetNumber("game", pn.ToString()+"_speed", 1);
        }

        public void OnPlanetClicked(PlanetName planetName) {
            Debug.Log("# OnPlanetClicked: "+planetName.ToString());
            clickedPlanets.Add(planetName);
            StartCoroutine(CheckLevelDone());
            /*if (planetName != PlanetName.none) {
                PlanetsMove(false);
            }*/
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
                            Debug.Log("# Level Advance");
                            CurrentLevelIndex++;
                            if(GamesManager.Instance!=null)
                                GamesManager.Instance.All[(int)GameData.GAMES.PHOTOS].LevelMain.LevelUp(GameData.GAMES.PHOTOS);
                            if (CurrentLevelIndex >= levels.Count)
                                CurrentLevelIndex = 0;
                            PlayerPrefs.SetInt("solar_system_level", CurrentLevelIndex);
                            OnLevelCompleted();
                        }
                    }
                }
            } else {
                OnPlanetDone(PlanetName.none);
            }
            yield return new WaitForEndOfFrame();
            clickedPlanets = new List<PlanetName>();
        }
    }

}