using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class MainScreen : MonoBehaviour
    {
        public SCREENS screen;
        public enum SCREENS
        {
            MAP,
            GAME
        }
        UIManager uiManager;
        public void Initialize(UIManager uiManager)
        {
            this.uiManager = uiManager;
            OnInitialize();
        }
        public void Show(bool isOn)
        {
            gameObject.SetActive(isOn);
            if (isOn)
                OnShow();
            else
                OnHide();
        }
        public virtual void OnInitialize() { }
        public virtual void OnShow()   {  }
        public virtual void OnHide()   {  }
    }
}
