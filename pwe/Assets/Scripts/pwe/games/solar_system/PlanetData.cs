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
        public List<Color> particleColors;
        public Texture2D lastPhoto;
        public bool hasNewPhoto;
    }
}