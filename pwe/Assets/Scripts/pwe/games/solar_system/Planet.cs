using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    [RequireComponent(typeof(Animator))]
    public class Planet : OrbitalItem
    {
        private PlanetData _planetData;
        private Animator _anim;
        private void Awake() {
            _anim = GetComponent<Animator>();
        }

        public void Init(Transform sun, SpaceData spaceData, PlanetData pd) {
            base.Init((int)pd.planetName, sun, spaceData, pd.orbitData, pd.sprite);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            Init(spaceData, pd);
        }
        public void Init(SpaceData spaceData, PlanetData pd, float colliderRadius, OrbitalPath path, System.Action onClick) {
            base.Init((int)pd.planetName, spaceData, pd.orbitData, pd.sprite, colliderRadius, path, onClick);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            Init(spaceData, pd);
        }

        void Init(SpaceData spaceData, PlanetData pd) {
            transform.localScale = spaceData.GetSize(pd.size);
            _planetData = pd;
            _anim.Play("planet_" +Random.Range(1,6));
        }

        private void Update() {
            //Orbit();
            if (Moving)
                OrbitCurve();
        }
    }
}