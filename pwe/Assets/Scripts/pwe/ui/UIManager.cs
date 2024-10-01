using Pwe.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] MainScreen[] allMainScreens;
        MainScreen mainScreen;
        private void Start()
        {
            foreach(MainScreen m in allMainScreens)
            {
                m.Initialize(this);
                m.Show(false);
            }
            Events.OnPlayGame += OnPlayGame;
            Events.ExitGame += ExitGame;
            ShowMap();
        }
        private void OnDestroy()
        {
            Events.OnPlayGame -= OnPlayGame;
            Events.ExitGame -= ExitGame;
        }

        private void ExitGame()
        {
            ShowMap();
        }

        void ShowMap()
        {
            OnShowScreen(MainScreen.SCREENS.MAP);
        }
        private void OnPlayGame(GamesManager.GAMES obj)
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
