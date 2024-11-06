using Codice.CM.Client.Differences.Merge;
using System;
using UnityEngine;

namespace Pwe.Core
{
    [Serializable]
    public class GameData
    {
        public enum GAMES
        {
            COOKING,
            PHOTOS
        }
        public GAMES game;
        public GameObject gameGO;
        public int level;

        public void LevelUp()
        {
            Debug.Log("LevelUp");
            level++;
        }
        public void Reset()
        {
            level = 0;
        }
    }
}