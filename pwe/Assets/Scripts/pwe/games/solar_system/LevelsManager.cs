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

        public event System.Action<PlanetName> OnPlanetDone;

        void Start() {
            clickedPlanets = new List<PlanetName>();
        }

        public SpaceData GetCurrentLevel() {
            return levels[CurrentLevelIndex];
        }

        public void OnPlanetClicked(PlanetName planetName) {
            clickedPlanets.Add(planetName);
            StartCoroutine(CheckLevelDone());
        }

        IEnumerator CheckLevelDone() {
            yield return new WaitForEndOfFrame();
            if (clickedPlanets.Count == 1) {
                if(OnPlanetDone!=null)
                    OnPlanetDone(clickedPlanets[0]);
            }
            yield return new WaitForEndOfFrame();
            clickedPlanets = new List<PlanetName>();
        }
    }
}