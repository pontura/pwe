using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;

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
        private void Start()
        {
            riveTexture.Init();
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
