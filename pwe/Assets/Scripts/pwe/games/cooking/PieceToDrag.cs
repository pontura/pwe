using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class PieceToDrag : DragElement
    {
        [SerializeField] CookingMainPiece mainPiece;
        RiveTexture riveTexture;
        void Start()
        {
            riveTexture = GetComponent<RiveTexture>();
            riveTexture.Init("pwa-ingredient.riv", OnReady);
        }
        public override void OnInitDrag()
        {
            Debug.Log(riveTexture + " OnInitDrag " + mainPiece.Ingredient);
            riveTexture.SetTrigger(mainPiece.Ingredient);
        }
        void OnReady() { }
        public override void OnEndDrag()
        {
            mainPiece.OnPieceReleased();
            transform.position = new Vector2(1000, 0);
        }
    }
}
