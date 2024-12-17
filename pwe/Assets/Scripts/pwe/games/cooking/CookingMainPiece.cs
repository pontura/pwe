using Pwe.Core;
using UnityEngine;
using UnityEngine.UI;
using Yaguar.Inputs2D;
using YaguarLib.Audio;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        [SerializeField] Animation anim;
        public Cooking cooking;
        //RiveRawImage riveTexture;
        [SerializeField] Image image;
        bool pieceOver;
        bool wasInitialized;
        public bool WasInit() { return wasInitialized; }

        public string Ingredient { get { return cooking.itemDragging.ToString(); }  }

        public void Init(string basePiece)
        {
            wasInitialized = true;
            image.sprite = cooking.cookingData.GetBase(basePiece); // TO-DO:
        }
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
            Events.OnAddParticles(ParticlesManager.types.drop, Input.mousePosition, Ingredient);
            anim.Play();
        }
        void OnDropItemOut()
        {
            YaguarLib.Events.Events.PlayGenericSound(cooking.Game.Sounds.GetClip("ingredient_release").clip, AudioManager.channels.GAME);
            cooking.ResetDrag();
        }
    }
}
