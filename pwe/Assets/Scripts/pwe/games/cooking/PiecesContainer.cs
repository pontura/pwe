using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking
{
    public class PiecesContainer : InteractiveElement
    {
        RiveTexture riveTexture;
        [SerializeField] DragInputManager dragInputManager;
        [SerializeField] DragElement dragElement;

        private void Awake()
        {
            riveTexture = GetComponent<RiveTexture>();
        }
        void Start()
        {
            riveTexture.Init("pwa-bowl.riv");
        }
        public void Init(CookingData.Items item)
        {
            riveTexture.SetTrigger("remove");
        }
        public override void OnClicked()
        {
            riveTexture.SetTrigger("remove");
            Vector2 pos = Input.mousePosition;
            dragInputManager.ForceDrag(pos, dragElement);
            print("FORCE DRAG");
        }
        public void Add() // if drop item out:
        {
            riveTexture.SetTrigger("add");
        }
    }
}
