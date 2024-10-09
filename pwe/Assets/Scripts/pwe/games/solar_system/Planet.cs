using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class Planet : OrbitalItem
    {
        // Start is called before the first frame update
        public void Init(Transform sun, SpaceData spaceData, PlanetData pd) {
            base.Init(sun, spaceData, pd.orbitData);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            transform.localScale = spaceData.GetSize(pd.size);
        }        

        private void Update() {
            Orbit();
        }
    }
}