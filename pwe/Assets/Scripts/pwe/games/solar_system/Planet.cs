using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class Planet : OrbitalItem
    {
        // Start is called before the first frame update
        public void Init(Transform sun, SpaceData spaceData, PlanetData pd) {
            base.Init((int)pd.planetName, sun, spaceData, pd.orbitData, pd.sprite);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            transform.localScale = spaceData.GetSize(pd.size);
        }
        public void Init(SpaceData spaceData, PlanetData pd, OrbitalPath path) {
            base.Init((int)pd.planetName, spaceData, pd.orbitData, pd.sprite, path);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            transform.localScale = spaceData.GetSize(pd.size);
        }

        private void Update() {
            //Orbit();
            OrbitCurve();
        }
    }
}