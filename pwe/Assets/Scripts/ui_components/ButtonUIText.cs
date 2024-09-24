using UnityEngine;
using UnityEngine.UI;

namespace Meme.UI
{
    [RequireComponent(typeof(Button))]

    public class ButtonUIText : ButtonUI
    {
        [SerializeField] TMPro.TMP_Text field;
        public void Init(System.Action<int> OnClick, string text, int id = 0)
        {
            Init(OnClick, 0);
            SetText(text);
        }
        public void SetText(string text)
        {
            if (field != null)
                field.text = text;
        }
    }
}
