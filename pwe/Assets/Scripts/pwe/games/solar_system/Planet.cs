using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class Planet : OrbitalItem
    {
        private PlanetData _planetData;
        // Start is called before the first frame update
        public void Init(Transform sun, SpaceData spaceData, PlanetData pd) {
            base.Init((int)pd.planetName, sun, spaceData, pd.orbitData, pd.sprite);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            transform.localScale = spaceData.GetSize(pd.size);
            _planetData = pd;
        }
        public void Init(SpaceData spaceData, PlanetData pd, OrbitalPath path, System.Action onClick) {
            base.Init((int)pd.planetName, spaceData, pd.orbitData, pd.sprite, path, onClick);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            transform.localScale = spaceData.GetSize(pd.size);
            _planetData = pd;
        }

        private void Update() {
            //Orbit();
            if (Moving)
                OrbitCurve();
        }
    }
}