using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        [SerializeField] Cooking cooking;
        RiveTexture riveTexture;
        bool pieceOver;

        [SerializeField] string states_pizza;
        [SerializeField] string trigger;

        public void Init(string ingredient)
        {
            riveTexture = GetComponent<RiveTexture>();
            riveTexture.Init();
            InitIngredient(ingredient);
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
        public void InitIngredient(string ingredient)
        {
            riveTexture.SetTrigger(ingredient);
        }
        public void OnPieceReleased()
        {
            if (pieceOver == false) return;
            cooking.OnPieceAdded();
            Debug.Log("On Piece Released over!");
            riveTexture.SetTrigger("add");
            pieceOver = false;
        }
    }
}
