using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pwe.Games
{
    public class Game : MonoBehaviour
    {
        [SerializeField] GameMain[] screens;
        [SerializeField] int screenID;
        GameMain active;
        [SerializeField] string[] allRivNames;
        [SerializeField] CookingData cookingData;
        public CookingData CookingData { get { return cookingData; } }

        void Start()
        {
            foreach (GameMain gm in screens) gm.Initialize(this);
           
            if (allRivNames != null && allRivNames.Length > 0)
            {
                MainApp.Instance.riveFilesManager.PreloadRivs(allRivNames, AllRivesLoaded);
            }
            else
            {
                AllRivesLoaded();
            }
        }
        void AllRivesLoaded()
        {
            Debug.Log("All Rives Loaded");
            if (SceneManager.GetActiveScene().name == "Main")
                screenID = 0;
            SetScreen();
        }
        void SetScreen()
        {
            active = GetScreen();
            if(active != null)
                active.Init();
        }
        GameMain GetScreen()
        {
            if (screenID >= screens.Length) {
                Core.Events.GamePlayed();
            } else {
                if (screenID < 0)
                    screenID = 0;
                return screens[screenID];
            }
            return null;
        }
        public void Next()
        {
            if (active) active.Hide();
            screenID++;
            SetScreen();
        }

        public void Back() {
            Debug.Log("Back");
            if (active) active.Hide();
            screenID--;
            SetScreen();
        }
    }
}