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
        [SerializeField] protected SpriteRenderer _itemSR;
        [SerializeField] CircleCollider2D _itemCollider;
        protected Transform _sun;
        protected float _orbitPosition;
        protected OrbitalPath _path;

        protected System.Action<bool> onClick;

        float extentFactor = 0.5f;

        [field: SerializeField] public bool Moving { get; set; }

        public virtual void Init(int id, Transform sun, SpaceData spaceData, OrbitData od, Sprite sprite) {
            base.Init(id);
            _sun = sun;
            _spaceData = spaceData;
            orbitData = od;
            //transform.position = new Vector3(_sun.position.x + (od.distance*_spaceData.DistanceFactor), _sun.position.y, _sun.position.z);
            float distance = spaceData.GetDistance(od.distance);
            Debug.Log("#Distance: " + distance);
            transform.position = new Vector3(_sun.position.x + distance, _sun.position.y, _sun.position.z);            
            _itemSR.sprite = sprite;
            Orbit(Random.Range(0.1f, 360f));
        }

        public virtual void Init(int id, SpaceData spaceData, OrbitData od, Sprite sprite, float colliderRadius, OrbitalPath oPath, System.Action<bool> onclick) {
            base.Init(id);
            _spaceData = spaceData;
            orbitData = od;
            _itemSR.sprite = sprite;
            //_itemCollider.radius = (_itemSR.bounds.size*0.15f).magnitude;
            _itemCollider.radius = colliderRadius;
            _path = oPath;
            OrbitCurve(Random.value);
            onClick = onclick;
            Moving = true;
        }

        public virtual void Init(int id, SpaceData spaceData, OrbitalPath oPath, System.Action<bool> onclick) {
            base.Init(id);
            _spaceData = spaceData;
            _path = oPath;
            OrbitCurve(Random.value);
            onClick = onclick;
            Moving = true;
        }

        protected void Orbit(float time=0) {
            if (orbitData != null) {
                time = time == 0 ? Time.deltaTime : time;
                transform.RotateAround(_sun.position, _sun.transform.forward, time * _spaceData.GetSpeed(orbitData.speed));
            }
            //transform.RotateAround(_sun.position, _sun.transform.forward, orbitData.speed * Time.deltaTime * _spaceData.SpeedFactor);
        }

        protected void OrbitCurve(float position = 0) {
            if (orbitData != null) {
                _orbitPosition = position==0 ? _orbitPosition : position;
                _orbitPosition += _spaceData.GetSpeed(orbitData.speed) * Time.deltaTime;
                if (_orbitPosition > 1f)
                    _orbitPosition = 0f;
            }
            transform.localPosition = _path.FollowPath(_orbitPosition);
            //transform.RotateAround(_sun.position, _sun.transform.forward, orbitData.speed * Time.deltaTime * _spaceData.SpeedFactor);
        }

        private void Update() {
            if(Moving)
                OrbitCurve();
            //Orbit();
        }

        public void OnClicked(Vector3 min, Vector3 max) {
            if (onClick != null) {                
                //Debug.Log(((PlanetName)id).ToString()+" x: "+(_itemSR.bounds.center.x + extentFactor * _itemSR.bounds.extents.x) +" > "+max.x +" || y: "+(_itemSR.bounds.center.y + extentFactor * _itemSR.bounds.extents.y) +" > "+max.y +" || x: "+(_itemSR.bounds.center.x - extentFactor * _itemSR.bounds.extents.x) +" < "+min.x+" || y: "+(_itemSR.bounds.center.y + extentFactor * _itemSR.bounds.extents.y) +" < "+min.y);
                //Debug.Log(_itemSR.bounds.min.x + " "+_itemSR.bounds.min.y + ", " + _itemSR.bounds.max.x + " " + _itemSR.bounds.max.y);
                onClick(!(_itemSR.bounds.center.x + extentFactor * _itemSR.bounds.extents.x > max.x || _itemSR.bounds.center.y + extentFactor * _itemSR.bounds.extents.y > max.y || _itemSR.bounds.center.x - extentFactor * _itemSR.bounds.extents.x < min.x || _itemSR.bounds.center.y - extentFactor * _itemSR.bounds.extents.y < min.y));
            }
        }
    }
}
