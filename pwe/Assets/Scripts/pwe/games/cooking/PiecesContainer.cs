using UnityEngine;
using Yaguar.Inputs;
using Yaguar.Inputs2D;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking
{
    public class PiecesContainer : InteractiveElement
    {
        RiveTexture riveTexture;
        [SerializeField] DragInputManager dragInputManager;
        [SerializeField] DragElement dragElement;
        string actionKey;
        string actionKeyQty;
        int num;

        private void Awake()
        {
            riveTexture = GetComponent<RiveTexture>();
        }
        void Start()
        {
            riveTexture.Init("pwa-bowl.riv");
        }
        public void InitIngredient(CookingData.Items item, int num)
        {
            this.num = num;
            this.actionKey = item.ToString();
            actionKeyQty = actionKey + "_qty";
            riveTexture.SetTrigger(actionKey);
            riveTexture.SetNumber(actionKeyQty, num);
        }
        public override void OnClicked()
        {
            Remove();
            Vector2 pos = Input.mousePosition;
            dragInputManager.ForceDrag(pos, dragElement);
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
    }
}
