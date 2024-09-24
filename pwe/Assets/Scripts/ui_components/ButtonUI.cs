using UnityEngine;
using UnityEngine.UI;

namespace Meme.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField] AudioManager.types audioType;
        [HideInInspector] public Button button;

        public virtual void Init(System.Action<int> OnClick, int id)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => { OnClick(id); Events.OnPlaySound(audioType); });
        }
        public virtual void Init(System.Action OnClick)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => { OnClick(); Events.OnPlaySound(audioType); });
        }
        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveAllListeners();
        }
        public void OnSelected(bool isSelected)
        {
            if(isSelected)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }
}
