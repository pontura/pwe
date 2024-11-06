using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingItem : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMPro.TMP_Text field;
        public ItemData data;

        public void Init(ItemData data)
        {
            this.data = data;
            field.text = "x" + data.num;
        }

    }
}