using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class CameraPan : MonoBehaviour
    {
        [field: SerializeField] public Camera cam { get; private set; }
        [SerializeField] float speed;
        [SerializeField] Vector2 horizontalLimits;
        [SerializeField] Vector3 initialPosition;
        public bool Panning { get; set; }
        [SerializeField] bool goRight;

        [SerializeField] float turnFrameFactor;

        float _currentSpeed;
        float _turnTime;
        bool _isChangingDirection;
        // Start is called before the first frame update
        void Start() {

#if UNITY_ANDROID && !UNITY_EDITOR
            speed *= 15;
#endif

            if (cam == null)
                cam = Camera.main;

            _currentSpeed = speed;
            _turnTime = Mathf.PI * 0.5f;
        }

        // Update is called once per frame
        void Update() {
            if (Panning)
                Move();
        }

        void Move() {
            int direction = goRight ? 1 : -1;
            Vector3 pos = cam.transform.localPosition;
            cam.transform.localPosition = new Vector3(pos.x + (_currentSpeed * direction), pos.y, pos.z);
            if (_isChangingDirection)
                UTurn();
            else
                CheckLimits();
        }

        void UTurn() {
            _turnTime += Time.deltaTime* turnFrameFactor;
            _turnTime = Mathf.Min(1.5f*Mathf.PI, _turnTime);
            _currentSpeed = speed * Mathf.Sin(_turnTime);
            //Debug.Log("#_currentSpeed: " + _currentSpeed);
            if ((!goRight && cam.transform.localPosition.x > horizontalLimits.x) || (goRight && cam.transform.localPosition.x < horizontalLimits.y)) {
                goRight = !goRight;
                _currentSpeed = speed;
                _turnTime = Mathf.PI*0.5f;
                _isChangingDirection = false;
                //Debug.Log("#_isChangingDirection: " + _isChangingDirection);
            } 
        }

        void CheckLimits() {
            if (cam.transform.localPosition.x < horizontalLimits.x || cam.transform.localPosition.x > horizontalLimits.y) {
                _isChangingDirection = true;
                //Debug.Log("#_isChangingDirection: " + _isChangingDirection);
            }
                //goRight = !goRight;
        }

        public void ResetPosition() {
            cam.transform.localPosition = initialPosition;
        }
    }
}
