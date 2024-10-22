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
        [SerializeField] PiecesContainer piecesContainer;
        string ingredient;

        public void Init()
        {
            riveTexture = GetComponent<RiveTexture>();
            riveTexture.Init("pwa-pizza.riv", OnReady);
        }
        void OnReady() { }
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
        public void InitIngredient(string ingredient, int num)
        {
            this.ingredient = ingredient;
            riveTexture.SetNumber(ingredient, num);
        }
        public void OnPieceReleased()
        {
            if (pieceOver == false)
            {
                OnDropItemOut();
            }
            else {
                cooking.OnPieceAdded();
                Debug.Log("On Piece Released over!");
                riveTexture.SetTrigger("add");
                pieceOver = false;
            }
        }
        void OnDropItemOut()
        {
            piecesContainer.Add();
        }
    }
}
