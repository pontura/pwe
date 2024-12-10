using Pwe.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text versionField;
        [SerializeField] MainScreen[] allMainScreens;
        MainScreen mainScreen;
        private void Start()
        {
            versionField.text = "v. " + Application.version;
            foreach (MainScreen m in allMainScreens)
            {
                m.Initialize(this);
                m.Show(false);
            }
            Events.OnPlayGame += OnPlayGame;
            Events.ExitGame += ExitGame;
            Events.GamePlayed += GamePlayed;
            ShowMap();
        }
        private void OnDestroy()
        {
            Events.OnPlayGame -= OnPlayGame;
            Events.ExitGame -= ExitGame;
            Events.GamePlayed -= GamePlayed;
        }

        private void ExitGame()
        {
            ShowMap();
        }
        private void GamePlayed()
        {
            ShowMap();
        }

        void ShowMap()
        {
            OnShowScreen(MainScreen.SCREENS.MAP);
        }
        private void OnPlayGame(GameData.GAMES game)
        {
            OnShowScreen(MainScreen.SCREENS.GAME);
        }
        void OnShowScreen(MainScreen.SCREENS s)
        {
            if (mainScreen != null) mainScreen.Show(false);
            mainScreen = GetMainScreen(s);
            mainScreen.Show(true);
        }
        MainScreen GetMainScreen(MainScreen.SCREENS s)
        {
            foreach(MainScreen ms in allMainScreens)
            {
                if (ms.screen == s)
                    return ms;
            }
            Debug.LogError("No screen " + s);
            return null;
        }
    }
}
