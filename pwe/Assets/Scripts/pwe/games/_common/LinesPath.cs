
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pwe.Games.Common
{
    [RequireComponent(typeof(LineRenderer))]
    public class LinesPath : MonoBehaviour
    {
        [SerializeField] List<Transform> points;

        [SerializeField] Color _lineColor;

        [SerializeField] float angleThresh;

        [SerializeField] float _factor;

        private float _pathPosition;
        private LineRenderer _lineRenderer;        

        int _currentPoint;
        float _position;
        Vector3 _dir;
        float _angle;
        float _invMgt;

        void Start() {
            _lineRenderer = GetComponent<LineRenderer>();
            DrawPath();
            ResetNode();
        }

        public void ResetNode(bool positiveDirection = true) {
            _position = positiveDirection?0:1;
            _dir = points[_currentPoint + 1].position - points[_currentPoint].position;
            _angle = (360 + Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)%360;
            _invMgt = 1f / _dir.magnitude;
        }


        public Vector3 FollowPath(float position) {
            return Vector3.Lerp(points[_currentPoint].position, points[_currentPoint + 1].position, position);
        }        

        public void DrawPath() {
            if(_lineRenderer==null)
                _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.material.color = _lineColor;
            _lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++) {
                _lineRenderer.SetPosition(i, points[i].position);                
            }
        }

        public void ToggleLine() {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = !_lineRenderer.enabled;
        }


        public Vector3 GetPosition(Vector3 oldMousePos, Vector3 newMousePos) {
            //Debug.Log("oldMouse: " + oldMousePos + " newMouse: " + newMousePos);
            Vector3 mouseDir = newMousePos - oldMousePos;
            float mouseAngle = (360 + Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg)%360;
            Debug.Log("mouseDir: " + mouseDir + " mouseAngle: " + mouseAngle + "mouseMag: "+ mouseDir.magnitude);
            Debug.Log("pointsDir: " + _dir + " pointsAngle: " + _angle + "pointsMag: " + _dir.magnitude);
            //Debug.Log("Negative: " + ((360 + _angle - 180) % 360 - angleThresh) + " & " + ((360 + _angle - 180) % 360 + angleThresh));
            if (mouseAngle>_angle-angleThresh&& mouseAngle < _angle + angleThresh) {
                _position += _factor*mouseDir.magnitude * _invMgt;
                if (_position >= 1f) {
                    _currentPoint++;
                    if (_currentPoint > points.Count - 2)
                        _currentPoint = points.Count - 2;
                    else
                        ResetNode();
                }
            }else if (mouseAngle > (360+_angle - 180)%360 - angleThresh && mouseAngle < (360+_angle - 180)%360 + angleThresh) {
                _position -= _factor*mouseDir.magnitude * _invMgt;
                if (_position <= 0f) {
                    _currentPoint--;
                    if (_currentPoint < 0)
                        _currentPoint = 0;
                    else
                        ResetNode(false);
                }
            }
            return FollowPath(_position);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LinesPath))]
    class OrbitalPathEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            LinesPath linesPath = (LinesPath)target;
            if (GUILayout.Button("Draw Curve")) {
                linesPath.DrawPath();
            }
            if (GUILayout.Button("Toggle Curve")) {
                linesPath.ToggleLine();
            }
            if (GUILayout.Button("Reset Points")) {
                linesPath.ResetNode();
            }
        }
    }
#endif

}
