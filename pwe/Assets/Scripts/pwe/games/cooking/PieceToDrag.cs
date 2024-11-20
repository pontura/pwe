using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class PieceToDrag : DragElementUI
    {
        [SerializeField] CookingMainPiece mainPiece;
        RiveRawImage riveTexture;
        public void Init(System.Action OnReady, CookingMainPiece mainPiece)
        {
            this.mainPiece = mainPiece;
            riveTexture = GetComponent<RiveRawImage>();
            riveTexture.Init("pwa-ingredient.riv", OnReady);
        }
        public override void OnInitDrag()
        {
            Debug.Log(riveTexture);
            Debug.Log( " OnInitDrag " + mainPiece);
            Debug.Log( " Ingredient " + mainPiece.Ingredient);
            riveTexture.SetTrigger(mainPiece.Ingredient);
        }
        void OnReady() { }
        public override void OnEndDrag()
        {
            mainPiece.OnPieceReleased(this);
            //transform.position = new Vector2(1000, 0);
        }
    }
}
