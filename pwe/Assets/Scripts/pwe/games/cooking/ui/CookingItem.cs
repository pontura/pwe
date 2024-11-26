using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingItem : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text field;
        public ItemData data;
        [SerializeField] RiveRawImage riveTexture;

        public void Init(ItemData data)
        {
            this.data = data;
            field.text = "x" + data.num;
            riveTexture.Init("Cooking/ingredient.riv", OnReady);
        }
        void OnReady() { 
            Debug.Log("BTN OnInitDrag " + data.item.ToString());
            riveTexture.SetTrigger(data.item.ToString());
        }
    }
}