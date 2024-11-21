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
            PHOTOS,
            ABC
        }
        public GAMES game;
        public GameObject gameGO;
        public int level;

        public void LevelUp()
        {
            Debug.Log("LevelUp");
            level++;
            PlayerPrefs.SetInt(game.ToString(), level);
            Events.GameLeveled(game, level);
        }
        public void Init()
        {
            level = PlayerPrefs.GetInt(game.ToString(), level);
        }
        public void Reset()
        {
            level = 0;
        }
    }
}