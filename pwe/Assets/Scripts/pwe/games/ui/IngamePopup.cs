using UnityEngine;
using YaguarLib.UI;

namespace Pwe.Games.UI
{
    public class IngamePopup : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI title;
        [SerializeField] ButtonUI continueButton;

        bool _activeSelf;
        private void Start() {
            gameObject.SetActive(true);
            Invoke(nameof(SetActive), Time.deltaTime*2);
        }

        public void Popup(string title, float delay = 0, System.Action onContinue=null) {
            Debug.Log("#Popup");
            this.title.text = title;
            continueButton.Init(() => {
                gameObject.SetActive(false);
                if (onContinue != null)
                    onContinue();
            });
            if (delay > 0) {
                _activeSelf = true;
                Invoke(nameof(SetActive), delay);
            } else
                gameObject.SetActive(true);
        }

        void SetActive() {
            Debug.Log("#SetActive");
            gameObject.SetActive(_activeSelf);
        }
    }
}