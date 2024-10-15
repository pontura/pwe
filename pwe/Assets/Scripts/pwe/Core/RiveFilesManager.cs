using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Pwe.Core
{
    public class RiveFilesManager : MonoBehaviour
    {
        const string serverURL = "https://pontura.github.io/pwe/pwe/Assets/Rive/";

        Dictionary<string, byte[]> all;
        private void Awake()
        {
            all = new Dictionary<string, byte[]>();
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
            string finalURL = serverURL + riveFile;
            Debug.Log("load: " + finalURL);
            UnityWebRequest www = UnityWebRequest.Get(finalURL);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download .rive file: " + www.error);
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
        public void PreloadRivs(List<string> rives, System.Action AllPreloaded)
        {
            this.AllPreloaded = AllPreloaded;
            totalLoaded = 0;
            totalRives = rives.Count;
            foreach (string s in rives)
            {
                if (all.ContainsKey(s))
                {
                    totalLoaded++;
                    Debug.Log("Was loaded: " + s + " - " + totalLoaded + " of "  + totalRives);
                }
                else
                {
                    Debug.Log("Loading: " + s + " - " + totalLoaded + " of " + totalRives);
                    string finalURL = s;
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
            if (totalLoaded >= totalRives)
                AllPreloaded();
        }
    }

}