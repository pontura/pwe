using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class CutScreen: GameMain
    {
        [SerializeField] GameObject dragAsset;
        [SerializeField] SpriteRenderer lineCut;
        [SerializeField] Slider slider;

        [SerializeField] bool right_left = true;
        [SerializeField] float lineCutSize = 4.3f;
        [SerializeField] float totalDragX = 4;
        [SerializeField] float delayToEnd = 1;
        float smooth = 10;

        public float v;
        states state;
        enum states
        {
            playing,
            done
        }

        public override void OnInitialize()
        {

        }
        public override void OnInit()
        {
            state = states.playing;
            v = 1;
            slider.value = 1;
            lineCut.gameObject.SetActive(true);
        }
        public override void OnUpdate()
        {
            if (state == states.playing)
            {
                v = slider.value;
                if (v <= 0)
                    Done();
            }
            SetValues();
        }
        void Done()
        {
            v = 0;
            lineCut.gameObject.SetActive(false);
            state = states.done;
            Invoke("Skip", delayToEnd);
        }
        void SetValues()
        {
            float _size = Mathf.Lerp(lineCutSize, 0, 1 - v);
            lineCut.size = Vector2.Lerp(lineCut.size, new Vector2(_size, lineCut.size.y), smooth * Time.deltaTime);
            Vector3 pos = dragAsset.transform.localPosition;
            float _x = Mathf.Lerp(-totalDragX / 2, totalDragX / 2, v);
            dragAsset.transform.localPosition = Vector3.Lerp(dragAsset.transform.localPosition, new Vector3(_x, pos.y, pos.z), smooth * Time.deltaTime);
        }
        void Skip()
        {
            Next();
        }
    }
}