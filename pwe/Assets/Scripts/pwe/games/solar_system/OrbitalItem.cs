using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pwe.Games.SolarSystem.PlanetsData;
using Yaguar.Inputs2D;

namespace Pwe.Games.SolarSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class OrbitalItem : InteractiveElement
    {
        [SerializeField] protected OrbitData orbitData;
        protected SpaceData _spaceData;
        SpriteRenderer _sr;
        protected Transform _sun;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
        }

        public virtual void Init(int id, Transform sun, SpaceData spaceData, OrbitData od, Sprite sprite) {
            base.Init(id);
            _sun = sun;
            _spaceData = spaceData;
            orbitData = od;
            //transform.position = new Vector3(_sun.position.x + (od.distance*_spaceData.DistanceFactor), _sun.position.y, _sun.position.z);
            float distance = spaceData.GetDistance(od.distance);
            Debug.Log("#Distance: " + distance);
            transform.position = new Vector3(_sun.position.x + distance, _sun.position.y, _sun.position.z);            
            _sr.sprite = sprite;
            Orbit(Random.Range(0.1f, 360f));
        }
        protected void Orbit(float time=0) {
            if (orbitData != null) {
                time = time == 0 ? Time.deltaTime : time;
                transform.RotateAround(_sun.position, _sun.transform.forward, time * _spaceData.GetSpeed(orbitData.speed));
            }
            //transform.RotateAround(_sun.position, _sun.transform.forward, orbitData.speed * Time.deltaTime * _spaceData.SpeedFactor);
        }

        private void Update() {
            Orbit();
        }
    }
}
