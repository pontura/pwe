using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        [SerializeField] Animation anim;
        [SerializeField] Cooking cooking;
        RiveRawImage riveTexture;
        bool pieceOver;

        [SerializeField] string states_pizza;
        [SerializeField] string trigger;
        [SerializeField] PiecesContainer piecesContainer;

        public void SetPieceContainer(PiecesContainer piecesContainer)
        {
            this.piecesContainer = piecesContainer;
        }

        public string Ingredient { get { return piecesContainer.ActionKey; }  }
        System.Action OnLoaded;

        public void Init(System.Action OnLoaded)
        {
            this.OnLoaded = OnLoaded;
            riveTexture = GetComponent<RiveRawImage>();
            riveTexture.Init("pwa-pizza.riv", OnReady);
        }
        void OnReady() { OnLoaded();  }
        public override void OnIECollisionEnter(InteractiveElement ie)
        {
            pieceOver = true;
        }
        public override void OnIECollisionExit(InteractiveElement ie)
        {
            pieceOver = false;
        }
        public void InitIngredient(string key, int num)
        {
            riveTexture.SetNumber(key, num);
        }
        public void OnPieceReleased(PieceToDrag pieceToDrag)
        {
            if (pieceOver == false)
            {
                OnDropItemOut();
                Destroy(pieceToDrag.gameObject);
            }
            else
            {
                cooking.OnPieceAdded(Ingredient);
                Add();
                pieceOver = false;
                pieceToDrag.transform.SetParent(transform);
                pieceToDrag.GetComponent<Collider2D>().enabled = false;
            }
        }
        public void Add()
        {
            anim.Play();
        }
        void OnDropItemOut()
        {
            piecesContainer.Add();
        }
    }
}
