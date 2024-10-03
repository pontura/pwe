using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pwe.Games
{
    public class Game : MonoBehaviour
    {
        [SerializeField] GameMain[] screens;
        [SerializeField] int screenID;
        GameMain active;

        void Start()
        {
            foreach (GameMain gm in screens) gm.Initialize(this);

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
            if(screenID >= screens.Length)
            {
                Core.Events.ExitGame();
               // Debug.LogError("No more screens");
            }
            else
                return screens[screenID];
            return null;
        }
        public void Next()
        {
            if (active) active.Hide();
            screenID++;
            SetScreen();
        }
    }
}