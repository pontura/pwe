using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaguarLib.UI
{
    public class Tab : MonoBehaviour
    {
        public void SetTab(bool isOn)
        {
            gameObject.SetActive(isOn);
            OnSetTab(isOn);
        }
        public virtual void OnSetTab(bool isOn) { }
        public void Refresh()
        {
            SetTab(true);
        }

    }
}
