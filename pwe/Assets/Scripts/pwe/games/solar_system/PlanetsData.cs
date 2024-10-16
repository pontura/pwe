using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Pwe.Games.SolarSystem
{
    public class PlanetsData : MonoBehaviour
    {
        public List<PlanetData> planets;

        public Vector2 GetMinMaxSize() {
            IEnumerable<PlanetData> orderedList = planets.OrderBy(x => x.size);
            Vector2 minmax = new Vector2(orderedList.ElementAtOrDefault(0).size, orderedList.ElementAtOrDefault(orderedList.Count() - 1).size);
            return minmax;
        }

        public Vector2 GetMinMaxDistance() {
            IEnumerable<PlanetData> orderedList = planets.OrderBy(x => x.orbitData.distance);
            Vector2 minmax = new Vector2(orderedList.ElementAtOrDefault(0).orbitData.distance, orderedList.ElementAtOrDefault(orderedList.Count() - 1).orbitData.distance);
            return minmax;
        }

        public Vector2 GetMinMaxSpeed() {
            IEnumerable<PlanetData> orderedList = planets.OrderBy(x => x.orbitData.speed);
            Vector2 minmax = new Vector2(orderedList.ElementAtOrDefault(0).orbitData.speed, orderedList.ElementAtOrDefault(orderedList.Count() - 1).orbitData.speed);
            return minmax;
        }
    }
    public enum PlanetName
    {
        none=0,
        mercury = 1,
        venus = 2,
        earth = 3,
        mars = 4,
        jupiter = 5,
        saturn = 6,
        uranus = 7,
        neptune = 8
    }    
}