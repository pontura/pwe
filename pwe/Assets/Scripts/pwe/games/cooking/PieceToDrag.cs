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
        public override void OnInitDrag()
        {
            Debug.Log("OnInitDrag");
        }
        public override void OnEndDrag()
        {
            mainPiece.OnPieceReleased();
            transform.position = new Vector2(1000, 0);
        }
    }
}
