using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Pwe.Core
{
    public class RiveFilesManager : MonoBehaviour
    {
        [SerializeField] bool useLocalRives;
        const string serverURL = "https://pontura.github.io/pwe/pwe/Assets/Rive/";

        Dictionary<string, byte[]> all;
        private void Awake()
        {
            all = new Dictionary<string, byte[]>();
#if !UNITY_EDITOR
            useLocalRives = false;
#endif
            Events.Reset += Reset;
        }
        private void OnDestroy()
        {
            Events.Reset -= Reset;
        }
        public void Reset()
        {
            foreach (string s in all.Keys)
            {
                byte[] arr = all[s];
                arr = null;
            }
            all.Clear();
        }


        public void Load(string riveFile, System.Action<byte[], string> OnDone)
        {
            if (all.ContainsKey(riveFile))
            {
                OnDone( all[riveFile], riveFile);
            }
            else
            {
                string finalURL = riveFile;
                StartCoroutine(LoadC(finalURL, OnDone));
            }
        }
        IEnumerator LoadC(string riveFile, System.Action<byte[], string> OnDone)
        {
            string finalURL;
            if (useLocalRives)
                finalURL = Application.dataPath + "/Rive/" + riveFile;
            else
                finalURL = serverURL + riveFile;

            Debug.Log("load: " + finalURL);
            UnityWebRequest www = UnityWebRequest.Get(finalURL);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download .rive file: " + www.error + " - rive file: " + riveFile);
                OnDone(null, "");
            }
            else
            {
                if (all.ContainsKey(riveFile))
                {
                    OnDone(all[riveFile], riveFile);
                }
                else
                {
                    all.Add(riveFile, www.downloadHandler.data);
                    OnDone(www.downloadHandler.data, riveFile);
                }
            }
        }


        //PRELOAD:
        int totalLoaded;
        int totalRives;
        System.Action AllPreloaded;
        public void PreloadRivs(string[] rives, System.Action AllPreloaded)
        {
            this.AllPreloaded = AllPreloaded;
            totalLoaded = 0;
            totalRives = rives.Length;
            foreach (string s in rives)
            {
                string riveName = s + ".riv";
                if (all.ContainsKey(riveName))
                {
                    Debug.Log("Load done: " + s);
                    totalLoaded++;
                    OnPreloaded(all[riveName], riveName);
                }
                else
                {
                    string finalURL = riveName;
                    StartCoroutine(LoadC(finalURL, OnPreloaded));
                }
            }
        }
        void OnPreloaded(byte[] data, string riveName)
        {
            totalLoaded++;
            CheckIfAllPreloaded();
        }
        void CheckIfAllPreloaded()
        {
            Debug.Log("Loaded: " + totalLoaded + " of " + totalRives);
            if (totalLoaded >= totalRives)
                AllPreloaded();
        }
    }

}