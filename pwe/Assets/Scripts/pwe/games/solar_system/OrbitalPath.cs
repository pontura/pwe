using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pwe.Games.SolarSystem
{
    [RequireComponent(typeof(LineRenderer))]
    public class OrbitalPath : MonoBehaviour
    {
        [SerializeField] Transform initPoint;
        [SerializeField] Transform endPoint;
        [SerializeField] Transform controlPoint;
        
        [SerializeField] int lineResolution = 200;
        [SerializeField] Color _lineColor;

        private float _pathPosition;
        private LineRenderer _lineRenderer;



        void Start() {
            _lineRenderer = GetComponent<LineRenderer>();
        }


        public Vector3 FollowPath(float position) {
            Vector3 m1 = Vector3.Lerp(initPoint.position, controlPoint.position, position);
            Vector3 m2 = Vector3.Lerp(controlPoint.position, endPoint.position, position);
            return Vector3.Lerp(m1, m2, position);
        }

        public void DrawBezierCurve() {
            if(_lineRenderer==null)
                _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.material.color = _lineColor;
            _lineRenderer.positionCount = lineResolution;
            float invCount = 1f / _lineRenderer.positionCount;
            Vector3 B = new Vector3(0, 0, 0);
            for (int i = 0; i < _lineRenderer.positionCount; i++) {
                _lineRenderer.SetPosition(i, FollowPath(i * invCount));                
            }
        }

        public void ToggleLine() {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = !_lineRenderer.enabled;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OrbitalPath))]
    class OrbitalPathEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            OrbitalPath orbitalPath = (OrbitalPath)target;
            if (GUILayout.Button("Draw Curve")) {
                orbitalPath.DrawBezierCurve();
            }
            if (GUILayout.Button("Toggle Curve")) {
                orbitalPath.ToggleLine();
            }
        }
    }
#endif

}
