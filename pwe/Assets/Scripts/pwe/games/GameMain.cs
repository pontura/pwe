using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pwe.Games
{
    public class GameMain : MonoBehaviour
    {
        private void Awake()
        {
            if (SceneManager.GetActiveScene().name != "Main")
                OnInit();
        }
        public virtual void OnInit()  { }

    }
}
