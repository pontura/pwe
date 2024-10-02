using UnityEngine;
using Yaguar.Inputs;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        RiveController riveController;
        bool pieceOver;
        private void Awake()
        {
            riveController = GetComponent<RiveController>();
        }
        public override void OnIECollisionEnter(InteractiveElement ie)
        {
            pieceOver = true;
            Debug.Log("Piece is over!");
        }
        public override void OnIECollisionExit(InteractiveElement ie)
        {
            pieceOver = false;
            Debug.Log("Piece is out...");
        }
        public void OnPieceReleased()
        {
            if (pieceOver == false) return;

            Debug.Log("On Piece Released over!");
            riveController.Clicked();
            pieceOver = false;
        }
    }
}
