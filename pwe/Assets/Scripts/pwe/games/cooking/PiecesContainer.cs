using Pwe.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.Cooking
{
    public class PiecesContainer : MonoBehaviour
    {
        // Animation anim;
        MultiRiveRawImage riveTexture;
        string actionKey;
        public string ActionKey { get { return actionKey; } }
        string actionKeyQty;
        int num;
        Cooking cooking;
        [SerializeField] TMPro.TMP_Text field;
        CookingMainPiece mainPiece;
        [SerializeField] GameObject signalNum;
        [SerializeField] GameObject signalNumDone;
        int itemID;
        ItemData.Items item;

        int _elementIndex;

        private void Awake()
        {
            signalNumDone.SetActive(false);
            //riveTexture = GetComponent<RiveRawImage>(); 
          //  anim = GetComponent<Animation>();
        }
        public void Initialize(MultiRiveRawImage riveTexture, int itemID, ItemData.Items item, Cooking cooking, CookingMainPiece mainPiece)
        {
            this.riveTexture = riveTexture;
            this.item = item;
            this.cooking = cooking;
            this.itemID = itemID;
            this.mainPiece = mainPiece;
            //riveTexture.Init("Cooking/bowls.riv", OnReady);
            riveTexture.LoadArtboard("Cooking/bowls.riv", t: transform, OnReady: OnReady);

        }
        public void SetSignalQty(int qty)
        {
            //signalNum.SetActive(qty > 0);
            if(qty>0)
                field.text = qty.ToString();
        }
        void OnReady(int index)
        {
            _elementIndex = index;
            InitIngredient(item, 10);
        }
        public void InitIngredient(ItemData.Items item, int num)
        {
            this.num = num;
            this.actionKey = item.ToString();
            actionKeyQty = actionKey + "_qty";
            riveTexture.SetTrigger(_elementIndex, actionKey);
            riveTexture.SetNumber(_elementIndex, actionKeyQty, num);
        }
        public void OnClicked()
        {
            mainPiece.SetPieceContainer(this);
            if (!cooking.CanMove()) return;

            Events.OnAddParticles(ParticlesManager.types.pick, Input.mousePosition);
            Remove();
            Vector2 pos = Input.mousePosition;
            cooking.InitDrag(item);
        }
        public void Add() // if drop item out:
        {
            num++;
            riveTexture.SetNumber(_elementIndex, actionKeyQty, num);
        }
        public void Remove() // if drop item out:
        {
            num--;
            riveTexture.SetNumber(_elementIndex, actionKeyQty, num);
        }
        public void AnimIn()
        {
          //  anim.Play("in");
        }
        public void AnimOut()
        {
           // anim.Play("out");
        }
        public void CheckIngredientReady(string ingredient)
        {
          //  signalNumDone.SetActive(ingredient == actionKey);
        }
    }
}
