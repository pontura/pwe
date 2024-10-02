using UnityEngine;
using UnityEngine.UI;

namespace Pwe.Games.Cooking.UI
{
    public class NumFeedback : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text field;
        [SerializeField] GameObject panel;

        private void Start()
        {
            Reset();
        }
        public void Init(int num)
        {
            panel.SetActive(false);
            CancelInvoke();
            panel.SetActive(true);
            field.text = num.ToString();
            Invoke("Reset", 1);
        }
        private void Reset()
        {
            panel.SetActive(false);
        }

    }
}