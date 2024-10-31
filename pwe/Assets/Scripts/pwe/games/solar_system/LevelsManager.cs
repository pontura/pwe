using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] List<SpaceData> levels;

        [field:SerializeField] public int CurrentLevelIndex { get; private set; }

        [SerializeField] private List<PlanetName> clickedPlanets;

        [SerializeField] private List<PlanetName> levelPlanets;

        public event System.Action<PlanetName> OnPlanetDone;
        public event System.Action OnLevelCompleted;

        void Start() {
            clickedPlanets = new List<PlanetName>();
        }

        public SpaceData InitLevel() {
            levelPlanets = new();
            foreach(SpaceData.LevelItem li in levels[CurrentLevelIndex].LevelItems) {
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
                        if (levelPlanets.Count == 0 && OnLevelCompleted != null)
                            OnLevelCompleted();
                    }
                }                
            }
            yield return new WaitForEndOfFrame();
            clickedPlanets = new List<PlanetName>();
        }
    }
}