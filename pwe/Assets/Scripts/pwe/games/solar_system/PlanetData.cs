using System;
using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.SolarSystem
{
    [Serializable]
    public class PlanetData
    {
        public PlanetName planetName;
        public OrbitData orbitData;
        public float size;
        public Sprite sprite;
        public Color color;
        public Texture2D lastPhoto;
        public bool levelCompleted;
        public bool hasNewPhoto;
    }
}