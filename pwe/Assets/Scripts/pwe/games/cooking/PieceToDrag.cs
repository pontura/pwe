using Pwe.Core;
using UnityEngine;
using UnityEngine.UI;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class PieceToDrag : DragElementUI
    {
        [SerializeField] Animation anim;
        [SerializeField] CookingMainPiece mainPiece;
        [SerializeField] Image image;
        // RiveRawImage riveTexture;
        public void Init(System.Action OnReady, CookingMainPiece mainPiece)
        {
            this.mainPiece = mainPiece;
            //riveTexture = GetComponent<RiveRawImage>();
            //riveTexture.Init("Cooking/ingredient.riv", OnReady);
            image.sprite = mainPiece.cooking.cookingData.GetIngredient(mainPiece.Ingredient);
            OnReady();
        }
        public override void OnInitDrag()
        {
            anim.Play("get");
            //Debug.Log(riveTexture);
            //Debug.Log( " OnInitDrag " + mainPiece);
            //Debug.Log( " Ingredient " + mainPiece.Ingredient);
            //riveTexture.SetTrigger(mainPiece.Ingredient);
        }
        void OnReady() { }
        public override void OnEndDrag()
        {
            anim.Play("drop");
            mainPiece.OnPieceReleased(this);
            //transform.position = new Vector2(1000, 0);
        }
    }
}
