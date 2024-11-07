using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] float speed;
        [SerializeField] Vector2 horizontalLimits;
        [SerializeField] Vector3 initialPosition;
        public bool Panning { get; set; }
        [SerializeField] bool goRight;

        // Start is called before the first frame update
        void Start() {
            if (cam == null)
                cam = Camera.main;
        }

        // Update is called once per frame
        void Update() {
            if (Panning)
                Move();
        }

        void Move() {
            int direction = goRight ? 1 : -1;
            Vector3 pos = cam.transform.localPosition;
            cam.transform.localPosition = new Vector3(pos.x + (speed * direction), pos.y, pos.z);
            CheckLimits();
        }

        void CheckLimits() {
            if (cam.transform.localPosition.x < horizontalLimits.x || cam.transform.localPosition.x > horizontalLimits.y)
                goRight = !goRight;
        }

        public void ResetPosition() {
            cam.transform.localPosition = initialPosition;
        }
    }
}
