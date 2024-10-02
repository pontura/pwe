using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Meme
{
    public static class Texts
    {
        [Serializable] class Lang
        {
            public string en;
            public string es;
        }
        static Dictionary<string, Lang> all;
        static string lang = "en";

        public static void SetData(string text) {
            all = new Dictionary<string, Lang>();
            jsonNode = SimpleJSON.JSON.Parse(text);
            loaded = true;
            for (int a = 0; a < jsonNode["all"].Count; a++)
            {
                Lang lang = new Lang();
                SimpleJSON.JSONNode item = jsonNode["all"][a];
                lang.en = item["en"];
                if (all.ContainsKey(item["id"]))
                    Debug.LogError("Repeated text id: " + item["id"]);
                else
                {
                    all.Add(item["id"], lang);
                }
            }
        }
        public static string Get(string key) {
            if (!loaded)
            {
                Debug.LogError("Texts are not being loaded yet. key: " + key);
                return "";
            }
            if(all.ContainsKey(key))
            {
                return all[key].en;
            }
            Debug.LogError("No text for " + key +" "+ lang);
            return "";
        }
        static bool loaded;
        static SimpleJSON.JSONNode jsonNode;
    }
}
