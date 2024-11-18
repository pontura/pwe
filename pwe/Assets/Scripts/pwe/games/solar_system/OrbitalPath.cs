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
        [SerializeField] Transform controlPoint1;
        [SerializeField] Transform controlPoint2;

        [SerializeField] int lineResolution = 200;
        [SerializeField] Color _lineColor;

        private float _pathPosition;
        private LineRenderer _lineRenderer;



        void Start() {
            _lineRenderer = GetComponent<LineRenderer>();
            DrawBezierCurve();
        }


        public Vector3 FollowPath(float position) {
            if (controlPoint2 == null) {
                return GetPoint(position);
            } else {
                return GetPoint(position, initPoint.position, controlPoint1.position, controlPoint2.position, endPoint.position);
            }
        }
        private Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
            float cx = 3 * (p1.x - p0.x);
            float cy = 3 * (p1.y - p0.y);
            float bx = 3 * (p2.x - p1.x) - cx;
            float by = 3 * (p2.y - p1.y) - cy;
            float ax = p3.x - p0.x - cx - bx;
            float ay = p3.y - p0.y - cy - by;
            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.x;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.y;

            return new Vector2(resX, resY);
        }

        private Vector3 GetPoint(float position) {
            Vector3 m1 = Vector3.Lerp(initPoint.position, controlPoint1.position, position);
            Vector3 m2 = Vector3.Lerp(controlPoint1.position, endPoint.position, position);
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
