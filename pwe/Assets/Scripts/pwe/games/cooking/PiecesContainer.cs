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
        public string ActionKey { get { return actionKey; } }
        string actionKeyQty;
        int num;
        Cooking cooking;

        private void Awake()
        {
            riveTexture = GetComponent<RiveTexture>();
        }
        public void Initialize()
        {
            riveTexture.Init("pwa-bowl.riv");
        }
        public void InitIngredient(Cooking cooking, ItemData.Items item, int num)
        {
            this.cooking = cooking;
            this.num = num;
            this.actionKey = item.ToString();
            actionKeyQty = actionKey + "_qty";
            riveTexture.SetTrigger(actionKey);
            riveTexture.SetNumber(actionKeyQty, num);
        }
        public override void OnClicked()
        {
            if (!cooking.CanMove()) return;
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
