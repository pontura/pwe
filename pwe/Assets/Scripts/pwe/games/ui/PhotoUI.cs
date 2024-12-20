using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Pwe.Games.UI
{
    public class PhotoUI : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] RectTransform frame;
        [SerializeField] RawImage photoTex;
        [SerializeField] GameObject flash;
        [SerializeField] GameObject fullFlash;
        [field: SerializeField] public float CloseDelay { get; private set; } //2f
        [field:SerializeField] public float FlyDelay { get; private set; } //0.3f
        [SerializeField] Animator anim;
        [SerializeField] Vector3 _flyToWrong = new Vector2(Screen.width, Screen.height);

        private Action onDone;

        bool _isRight;

        public void Init(Texture2D tex, Action callback) {
            //Debug.Log("#Init");
            fullFlash.SetActive(true);
            Invoke(nameof(TurnFlashOff), 0.1f);
            onDone = callback;
            photoTex.texture = tex;
            gameObject.SetActive(true);
            Invoke(nameof(OnClose), CloseDelay);            
        }        

        public void SetDone(bool enable) {
            _isRight = enable;
        }

        public void SetDelayedFly(bool enable) {
            _delayedFly = enable;
        }

        bool _delayedFly;
        bool _photoFly;
        bool _flyOnWrong;
        Vector3 _photoFlyToPos;
        
        public void FlyTo(Vector3 pos) {
            Debug.Log("#FlyTo: " + pos);
            _photoFlyToPos = pos;
            Debug.Log("#FlyTo: " + _photoFlyToPos);
            _photoFly = true;
        }

        public void FlyOnWrong(Vector3 pos) {
            Debug.Log("#FlyOnWrong: " + pos);
            _flyToWrong = pos;
            _flyOnWrong = true;
            _photoFlyToPos = new Vector3(_flyToWrong.x, _flyToWrong.y, _flyToWrong.z);
        }

        public void FadeSize(Vector2 initSize, Vector2 finalSize, float dur) {
            frame.sizeDelta = initSize;
            StartCoroutine(FadeVector(initSize, finalSize, dur, (vector) => frame.sizeDelta=vector));
        }

        public void FadePosition(Vector2 initPos, Vector2 finalPos, float dur) {                     

            //Vector2 initCenter = initPos - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 initCenter = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, initPos, cam, out initCenter);
            frame.localPosition = initCenter;
            FadePosition(finalPos, dur);
            //Vector2 finalCenter = finalPos - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        }

        public void FadePosition(Vector2 finalPos, float dur) {
            Vector2 finalCenter = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, finalPos, cam, out finalCenter);
            StartCoroutine(FadeVector(frame.localPosition, finalCenter, dur, (vector) => frame.localPosition = vector));
        }

        public void FadeAngle(Vector3 initAngle, Vector3 finalAngle, float dur) {
            frame.eulerAngles = initAngle;
            FadeAngle(finalAngle, dur);
        }

        public void FadeAngle(Vector3 finalAngle, float dur) {
            StartCoroutine(FadeVector(frame.eulerAngles, finalAngle, dur, (vector) => frame.eulerAngles = vector));
        }

        void TurnFlashOff() {
            fullFlash.SetActive(false);
        }

        void OnClose() {
            //Debug.Log("# Right: " + _isRight);
            //Debug.Log("# _delayedFly: " + _delayedFly);
            //Debug.Log("# _photoFly: " + _photoFly);
            string animName = _isRight ? "photo_right" : "photo_wrong";
            anim.Play(animName);
            //done.SetActive(_isRight);
            if (_delayedFly) {
                FadePosition(new Vector2(Screen.width * 0.5f, Screen.height * 0.65f), 0.2f);
                FadeAngle(Vector3.zero, 0.2f);
            } else if (_photoFly || _flyOnWrong) { 
                Fly();
            } else {
                Close();
            }
        }

        public void Fly() {
            //Debug.Log("#localPosition: " + frame.localPosition);
            //Debug.Log("#_photoFlyToPos: " + _photoFlyToPos);
            StartCoroutine(FadeVector(frame.position, _photoFlyToPos, FlyDelay, (vector) => frame.position = vector, Close));
            StartCoroutine(FadeVector(frame.eulerAngles, Vector3.zero, FlyDelay, (vector) => frame.eulerAngles = vector));
            StartCoroutine(FadeVector(frame.sizeDelta, new Vector2(60, 60), FlyDelay, (vector) => frame.sizeDelta = vector));
        }

        public void Close() {
            //Debug.Log("#Close");
            _photoFly = false;
            _delayedFly = false;
            _isRight = false;            
            gameObject.SetActive(false);
            onDone();
            if(_flyOnWrong)
                _photoFlyToPos = new Vector3(_flyToWrong.x, _flyToWrong.y, _flyToWrong.z);
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

        IEnumerator FadeVector(Vector3 init, Vector3 final, float dur, Action<Vector3> SetVector, Action onDone=null) {
            float startTime = Time.realtimeSinceStartup;
            float timeOut = Time.realtimeSinceStartup + dur;
            float invDur = 1f / dur;
            while (Time.realtimeSinceStartup < timeOut) {
                Vector3 result = Vector3.Lerp(init, final, (Time.realtimeSinceStartup - startTime) * invDur);
                SetVector(result);
                yield return new WaitForEndOfFrame();
            }
            if (onDone != null)
                onDone();
        }
    }
}