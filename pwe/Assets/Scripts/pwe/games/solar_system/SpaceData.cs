using System;
using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.SolarSystem
{
    [CreateAssetMenu(fileName = "NewSpaceData", menuName = "SolarSystem/Data/Space", order = 0)]
    public class SpaceData : ScriptableObject
    {        
        //[Range(1, 6)]
        [field: SerializeField] public float MinDistance { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; }

        public AnimationCurve distanceCurve;

        [field: SerializeField] public float MinSpeed { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }

        public AnimationCurve speedCurve;

        [field: SerializeField] public float MinSize { get; private set; }
        [field: SerializeField] public float MaxSize { get; private set; }

        public AnimationCurve sizeCurve;

        [field: SerializeField] public List<LevelItem> LevelItems { get; private set; }

        [field: SerializeField] public float orbitSpeedFactor;

        [HideInInspector]
        public Vector2 _minmaxSpeed;
        [HideInInspector]
        public Vector2 _minmaxSize;
        [HideInInspector]
        public Vector2 _minmaxDistance;

        [Serializable]
        public class LevelItem
        {
            public OrbitalItem orbitalItem;
            public OrbitalPath orbitalPath;
            public PlanetName planetName;
        }

        public float GetSpeed(float speed) {
            float index = (speed - _minmaxSpeed.x) / (_minmaxSpeed.y - _minmaxSpeed.x);
            return orbitSpeedFactor * (MinSpeed + speedCurve.Evaluate(index) * (MaxSpeed-MinSpeed));
        }

        public Vector3 GetSize(float size) {
            float index = (size - _minmaxSize.x) / (_minmaxSize.y - _minmaxSize.x);
            float new_size = MinSize + sizeCurve.Evaluate(index) * (MaxSize - MinSize);
            return new Vector3(new_size,new_size, new_size);
        }

        public float GetDistance(float distance) {
            float index = (distance - _minmaxDistance.x) / (_minmaxDistance.y - _minmaxDistance.x);
            return MinDistance + distanceCurve.Evaluate(index) * (MaxDistance - MinDistance);
        }
    }
}
