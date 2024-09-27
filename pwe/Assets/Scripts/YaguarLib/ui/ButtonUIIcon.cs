using UnityEngine;
using UnityEngine.UI;

namespace YaguarLib.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonUIIcon : ButtonUI
    {
        public TMPro.TMP_Text field;
        [SerializeField] Image icon;

        public void SetText(string text)
        {
            if (field != null)
                field.text = text;
        }
        public void SetIcon(Sprite s)
        {
            if(icon != null)
                icon.sprite = s;
        }
    }
}
