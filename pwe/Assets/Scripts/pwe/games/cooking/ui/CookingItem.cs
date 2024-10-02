using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingItem : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMPro.TMP_Text field;
        public CookingItemData data;

        public void Init(CookingItemData data)
        {
            this.data = data;
            field.text = "x" + data.num;
        }

    }
}