using UnityEngine;
using YaguarLib.UI;

namespace Pwe.Games.UI
{
    public class IngamePopup : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI title;
        [SerializeField] ButtonUI continueButton;

        private void Start() {
            gameObject.SetActive(false);
        }

        public void Popup(string title, float delay = 0, System.Action onContinue=null) {
            Debug.Log("#Popup");
            this.title.text = title;
            continueButton.Init(() => {
                gameObject.SetActive(false);
                if (onContinue != null)
                    onContinue();
            });
            if (delay > 0)
                Invoke(nameof(SetActive), delay);
            else
                gameObject.SetActive(true);
        }

        void SetActive() {
            Debug.Log("#SetActive");
            gameObject.SetActive(true);
        }
    }
}