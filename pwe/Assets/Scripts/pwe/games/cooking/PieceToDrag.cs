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
        public void Init(System.Action OnReady, CookingMainPiece mainPiece, int id)
        {
            this.mainPiece = mainPiece;
            //riveTexture = GetComponent<RiveRawImage>();
            //riveTexture.Init("Cooking/ingredient.riv", OnReady);
            string Ingredient = mainPiece.Ingredient;

            print("init drag " + Ingredient + " id: " + id);
            if (Ingredient == "flowers") // TO-DO ingredient per color:
                 { if (id > 5) id = id - 5; }
            else if (Ingredient == "macarons") // TO-DO ingredient per color:
                { if (id > 5) id = id - 5; }
            else if (Ingredient == "meringues") // TO-DO ingredient per color:
                { if (id > 5) id = id - 5; }
            else if (Ingredient == "gummybears")
            {
                switch (id)
                {
                    case 10:
                    case 7:
                    case 4:
                    default: id = 0; break;

                    case 9:
                    case 6:
                    case 3: id = 2; break;

                    case 8:
                    case 5:
                    case 2: id = 1; break;

                }
            }
            if (Ingredient == "tomatoes")
                image.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-30, 30));
            else
                image.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));

            image.sprite = mainPiece.cooking.cookingData.GetIngredient(mainPiece.Ingredient, id);
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
