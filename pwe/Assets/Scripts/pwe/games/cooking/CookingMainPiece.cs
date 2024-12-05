using Pwe.Core;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Cooking
{
    public class CookingMainPiece : InteractiveElement
    {
        [SerializeField] Animation anim;
        [SerializeField] Cooking cooking;
        [SerializeField] MultiRiveRawImage riveMultiTexture;
        RiveRawImage riveTexture;
        bool pieceOver;

        [SerializeField] PiecesContainer piecesContainer;

        int _elementIndex;

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
            riveTexture.Init("Cooking/pizza.riv", OnReady);
        }

        public void InitMulti(System.Action OnLoaded) {
            Debug.Log("% ACA");
            this.OnLoaded = OnLoaded;
            riveMultiTexture.LoadArtboard("Cooking/pizza.riv", t: transform, OnReady: OnReady);
        }

        void OnReady() { OnLoaded(); }

        void OnReady(int index) {
            Debug.Log("% StateMachine Index: " + index);
            _elementIndex = index; 
            OnLoaded();
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
            if(riveTexture!=null)
                riveTexture.SetNumber(key, num);
            if (riveMultiTexture!=null)
                riveMultiTexture.SetNumber(_elementIndex,key, num);
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
            piecesContainer.Add();
        }
    }
}
