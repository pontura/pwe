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

        public void Init(string ingredient)
        {
            this.ingredient = ingredient;
            riveTexture = GetComponent<RiveTexture>();
            riveTexture.Init("pwa-pizza.riv", OnReady);
        }
        void OnReady()
        {
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
