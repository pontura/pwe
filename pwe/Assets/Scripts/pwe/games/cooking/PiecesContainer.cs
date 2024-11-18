using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.Cooking
{
    public class PiecesContainer : MonoBehaviour
    {
       // Animation anim;
        RiveRawImage riveTexture;
        string actionKey;
        public string ActionKey { get { return actionKey; } }
        string actionKeyQty;
        int num;
        Cooking cooking;
        [SerializeField] TMPro.TMP_Text field;
        CookingMainPiece mainPiece;
        List<ItemData> items;
        [SerializeField] GameObject signalNum;
        [SerializeField] GameObject signalNumDone;
        int itemID;

        private void Awake()
        {
            signalNumDone.SetActive(false);
            riveTexture = GetComponent<RiveRawImage>(); 
          //  anim = GetComponent<Animation>();
        }
        public void Initialize(int itemID, List<ItemData> items, Cooking cooking, CookingMainPiece mainPiece)
        {
            this.cooking = cooking;
            this.itemID = itemID;
            this.items = items;
            this.mainPiece = mainPiece;
            riveTexture.Init("pwa-bowl.riv", OnReady);
        }
        public void SetSignalQty(int qty)
        {
            signalNum.SetActive(qty > 0);
            if(qty>0)
                field.text = qty.ToString();
        }
        void OnReady()
        {
            InitIngredient(items[itemID].item, 10);
        }
        public void InitIngredient(ItemData.Items item, int num)
        {
            this.num = num;
            this.actionKey = item.ToString();
            actionKeyQty = actionKey + "_qty";
            riveTexture.SetTrigger(actionKey);
            riveTexture.SetNumber(actionKeyQty, num);
        }
        public void OnClicked()
        {
            mainPiece.SetPieceContainer(this);
            if (!cooking.CanMove()) return;
            Remove();
            Vector2 pos = Input.mousePosition;
            cooking.InitDrag();
        }
        public void Add() // if drop item out:
        {
            num++;
            riveTexture.SetNumber(actionKeyQty, num);
        }
        public void Remove() // if drop item out:
        {
            num--;
            riveTexture.SetNumber(actionKeyQty, num);
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
            signalNumDone.SetActive(ingredient == actionKey);
        }
    }
}
