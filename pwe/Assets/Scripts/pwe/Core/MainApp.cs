using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Core
{
    public class MainApp : MonoBehaviour
    {
        public static MainApp Instance;

        public MainApp mInstance { get { return Instance; } }

        public RiveFilesManager riveFilesManager;

        private void Awake()
        {
            if (mInstance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist this object across scenes
            }
            riveFilesManager = GetComponent<RiveFilesManager>();
        }
    }

}