using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class PlanetsManager : MonoBehaviour
    {
        [SerializeField] SpaceData spaceData;
        [SerializeField] Transform sun;
        [SerializeField] Transform planetsContainer;
        [SerializeField] Planet planet_prefab;

        [SerializeField] List<Planet> planets;
        public void Init(PlanetsData planetsData) {
            spaceData._minmaxDistance = planetsData.GetMinMaxDistance();
            spaceData._minmaxSize = planetsData.GetMinMaxSize();
            spaceData._minmaxSpeed = planetsData.GetMinMaxSpeed();
        }

        public void AddPlanet(PlanetData pd) {
            Planet p = Instantiate(planet_prefab, planetsContainer);
            p.Init(sun, spaceData, pd);
        }

        public void RemoveAllPlanets() {
            foreach (Transform child in planetsContainer) {
                if(child!=sun)
                    Destroy(child.gameObject);
            }
        }
    }
}