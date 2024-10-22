using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    [RequireComponent(typeof(Animation))]
    public class Planet : OrbitalItem
    {
        private PlanetData _planetData;
        private Animation _anim;
        private void Awake() {
            _anim = GetComponent<Animation>();
        }

        public void Init(Transform sun, SpaceData spaceData, PlanetData pd) {
            base.Init((int)pd.planetName, sun, spaceData, pd.orbitData, pd.sprite);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            Init(spaceData, pd);
        }
        public void Init(SpaceData spaceData, PlanetData pd, float colliderRadius, OrbitalPath path, AnimationClip animClip, System.Action<bool> onClick) {
            base.Init((int)pd.planetName, spaceData, pd.orbitData, pd.sprite, colliderRadius, path, onClick);
            //transform.localScale = transform.localScale * pd.size * spaceData.SizeFactor;
            _anim.AddClip(animClip, animClip.name);            
            _anim.Play(animClip.name);
            Init(spaceData, pd);
        }

        void Init(SpaceData spaceData, PlanetData pd) {
            transform.localScale = spaceData.GetSize(pd.size);
            _itemSR.color = pd.color;
            _planetData = pd;            
        }

        private void Update() {
            //Orbit();
            if (Moving)
                OrbitCurve();
        }
    }
}