using UnityEngine;
using UnityEngine.UI;

namespace YaguarLib.UI
{
    [RequireComponent(typeof(Button))]

    public class ButtonUIText : ButtonUI
    {
        [SerializeField] TMPro.TMP_Text field;
        public void SetText(string text)
        {
            if (field != null)
                field.text = text;
        }
    }
}
