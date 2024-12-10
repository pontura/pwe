using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.Cooking.CookingData;

namespace Pwe.Games.Cooking.UI
{
    public class CookingItem : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text field;
        public ItemData data;
        [SerializeField] Image image;
        [SerializeField] Animator anim;
        public bool done;

        public void Init(ItemData data, Sprite s)
        {
            this.data = data;
            field.text = "x" + data.num;
            image.sprite = s;
            anim.Play("entry");
        }
        public void OnReady() {
            done = true;
            anim.Play("done");
        }
    }
}