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


        public string Ingredient { get { return cooking.itemDragging.ToString(); }  }
        System.Action OnLoaded;

        public void Init(System.Action OnLoaded, string basePiece)
        {
            this.OnLoaded = OnLoaded;
            image.sprite = cooking.cookingData.GetBase(basePiece); // TO-DO:
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
            Events.OnAddParticles(ParticlesManager.types.drop, Input.mousePosition, Ingredient);
            anim.Play();
        }
        void OnDropItemOut()
        {
            YaguarLib.Events.Events.OnPlaySoundInChannel(AudioManager.types.RELEASE, AudioManager.channels.UI);
            cooking.ResetDrag();
        }
    }
}
