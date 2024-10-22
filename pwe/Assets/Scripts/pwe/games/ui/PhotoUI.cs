using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Pwe.Games.UI
{
    public class PhotoUI : MonoBehaviour
    {
        [SerializeField] RectTransform frame;
        [SerializeField] RawImage photoTex;
        [SerializeField] GameObject flash;
        [SerializeField] GameObject fullFlash;
        [SerializeField] float _delay = 2;
        [SerializeField] GameObject done;

        private Action onDone;        

        public void Init(Texture2D tex, Action callback) {
            fullFlash.SetActive(true);
            Invoke(nameof(TurnFlashOff), 0.1f);
            onDone = callback;
            photoTex.texture = tex;
            gameObject.SetActive(true);
            Invoke(nameof(Close), _delay);            
        }

        public void SetDone(bool enable) {
            done.SetActive(enable);
        }

        public void FadeSize(Vector2 initSize, Vector2 finalSize, float dur) {
            frame.sizeDelta = initSize;
            StartCoroutine(FadeVector(initSize, finalSize, dur, (vector) => frame.sizeDelta=vector));
        }

        public void FadePosition(Vector2 initPos, Vector2 finalPos, float dur) {
            Vector2 initCenter = initPos - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 finalCenter = finalPos - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            frame.localPosition = initCenter;
            StartCoroutine(FadeVector(initCenter, finalCenter, dur, (vector) => frame.localPosition = vector));
        }

        public void FadeAngle(Vector3 initAngle, Vector3 finalAngle, float dur) {
            frame.eulerAngles = initAngle;
            StartCoroutine(FadeVector(initAngle, finalAngle, dur, (vector) => frame.eulerAngles = vector));
        }

        void TurnFlashOff() {
            fullFlash.SetActive(false);
        }

        void Close() {
            gameObject.SetActive(false);
            done.SetActive(false);
            onDone();
        }

        private void Update() {
            
        }

        IEnumerator FadeVector(Vector2 init, Vector2 final, float dur, Action<Vector2> SetVector) {
            float startTime = Time.realtimeSinceStartup;
            float timeOut = Time.realtimeSinceStartup+dur;
            float invDur = 1f / dur;
            while (Time.realtimeSinceStartup<timeOut) {
                Vector2 result = Vector2.Lerp(init, final, (Time.realtimeSinceStartup - startTime) * invDur);
                SetVector(result);
                yield return new WaitForEndOfFrame();
            }            
        }

        IEnumerator FadeVector(Vector3 init, Vector3 final, float dur, Action<Vector3> SetVector) {
            float startTime = Time.realtimeSinceStartup;
            float timeOut = Time.realtimeSinceStartup + dur;
            float invDur = 1f / dur;
            while (Time.realtimeSinceStartup < timeOut) {
                Vector3 result = Vector3.Lerp(init, final, (Time.realtimeSinceStartup - startTime) * invDur);
                SetVector(result);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}