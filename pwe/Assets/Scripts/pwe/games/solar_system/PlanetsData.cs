using System;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Games.SolarSystem
{
    public class PlanetsData : MonoBehaviour
    {
        public List<PlanetData> planets;

        public enum Planet
        {
            mercury=1,
            venus=2,
            earth=3,
            mars=4,
            jupiter=5,
            saturn=6,
            uranus=7,
            neptune=8
        }

        [Serializable]
        public class PlanetData
        {
            public Planet planet;
            public OrbitData orbitData;
            public float size;
        }

        [Serializable]
        public class OrbitData
        {
            public float speed;
            public float distance;
        }

    }

}