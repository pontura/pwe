using UnityEngine;
using UnityEngine.UI;
using System;

namespace Pwe.Games.UI
{
    public class PhotoUI : MonoBehaviour
    {
        [SerializeField] RawImage photoTex;
        [SerializeField] Image flash;
        [SerializeField] float _delay = 2;

        private Action onDone;

        public void Init(Texture2D tex, Action callback, Vector2 pos) {
            Vector2 center = pos - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            photoTex.transform.parent.localPosition = center;
            Init(tex, callback);
        }

        public void Init(Texture2D tex, Action callback) {
            flash.gameObject.SetActive(true);
            Invoke(nameof(TurnFlashOff), 0.1f);
            onDone = callback;
            photoTex.texture = tex;
            gameObject.SetActive(true);
            Invoke(nameof(Close), _delay);            
        }

        void TurnFlashOff() {
            flash.gameObject.SetActive(false);
        }

        void Close() {
            gameObject.SetActive(false);
            onDone();
        }
    }
}