using Pwe.Core;
using UnityEngine;
using UnityEngine.UI;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        [SerializeField] Animation anim;
        public Cooking cooking;
        //RiveRawImage riveTexture;
        [SerializeField] Image image;
        bool pieceOver;


        public string Ingredient { get { return cooking.itemDragging.ToString(); }  }
        System.Action OnLoaded;

        public void Init(System.Action OnLoaded)
        {
            this.OnLoaded = OnLoaded;
            //riveTexture = GetComponent<RiveRawImage>();
            //riveTexture.Init("Cooking/pizza.riv", OnReady);

            image.sprite = cooking.Data.GetBase("pizza"); // TO-DO:
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
           // riveTexture.SetNumber(key, num);
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
            Events.OnAddParticles(ParticlesManager.types.drop, Input.mousePosition);
            anim.Play();
        }
        void OnDropItemOut()
        {
            cooking.ResetDrag();
        }
    }
}
