using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class PiecesContainer : InteractiveElement
    {
        RiveController riveController;
        [SerializeField] DragInputManager dragInputManager;
        [SerializeField] DragElement dragElement;

        private void Awake()
        {
            riveController = GetComponent<RiveController>();
        }
        public override void OnClicked()
        {
            riveController.Clicked();
            Vector2 pos = Input.mousePosition;
            dragInputManager.ForceDrag(pos, dragElement);
            print("FORCE DRAG");
        }
    }
}
