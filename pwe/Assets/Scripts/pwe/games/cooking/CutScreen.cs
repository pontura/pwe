using UnityEngine;
using UnityEngine.SceneManagement;
using YaguarLib.UI;

namespace Pwe.Games.Cooking
{
    public class CutScreen: GameMain
    {
        [SerializeField] GameObject dragAsset;
        [SerializeField] SpriteRenderer lineCut;

        [SerializeField] bool right_left = true;
        [SerializeField] float lineCutSize = 4.3f;
        [SerializeField] float totalDragX = 4;
        [SerializeField] float value;

        public override void OnInitialize()
        {

        }
        public override void OnUpdate()
        {
            float _size = Mathf.Lerp(lineCutSize, 0, value);
            lineCut.size = new Vector2(_size, lineCut.size.y);
            Vector3 pos = dragAsset.transform.position;
            float _x = Mathf.Lerp(-totalDragX / 2, totalDragX / 2, value);
            dragAsset.transform.position = new Vector3(_x, pos.y, pos.z);
        }
        public override void OnInit()
        {
            base.OnInit();
        }
        void Skip()
        {
            Next();
        }
    }
}