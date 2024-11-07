using Pwe.Core;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using YaguarLib.UI;

namespace Pwe.UI
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] ButtonUI button;
        [SerializeField] GameObject panel;
        bool isOn;

        //Panel:
        [SerializeField] ButtonUIText resetBtn;
        void Start()
        {
            button.Init(OnToggleSettings);
            resetBtn.Init(OnReset);
            resetBtn.SetText("RESET");
            Show(false);
        }

        private void OnReset()
        {
            GamesManager.Instance.Reset();
            Show(false);
        }

        void OnToggleSettings()
        {
            isOn = !isOn;
            Show(isOn);
        }
        void Show(bool isOn)
        {
            this.isOn = isOn;
            panel.SetActive(isOn);
        }
    }
}
