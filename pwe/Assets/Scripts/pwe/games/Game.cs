using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rive;

namespace Pwe.Games
{
    public class Game : MonoBehaviour
    {
        public RiveTexture rive;
        [SerializeField] GameMain[] screens;
        [SerializeField] int screenID;
        GameMain active;
        // [SerializeField] string[] allRivNames;
        [SerializeField] string allRivName;
        [SerializeField] string rivFilePath;
        [SerializeField] List<string> artboards;

        void Start()
        {
            Application.targetFrameRate = 60;

            foreach (GameMain gm in screens) gm.Initialize(this);
           
        //    if (allRivName != "")
        //    {
        //        MainApp.Instance.riveFilesManager.PreloadRiv(allRivName, AllRivesLoaded);
        //    }
        //    else
        //    {
                AllRivesLoaded();
        //    }
        }
        public virtual void AllRivesLoaded() {
        //    if(rivFilePath!="")
        //        rive.Init(rivFilePath, OnLoaded);
        //    else
                InitScreens();
        }

        protected virtual void OnLoaded() {
            LoadAllArtBoards();
            InitScreens();            
        }        

        public void InitScreens()
        {
            Debug.Log("All Rives Loaded");
            if (SceneManager.GetActiveScene().name == "Main")
                screenID = 0;
            SetScreen();
        }
        public void LoadAllArtBoards()
        {
            rive.AddArtBoards(artboards);
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