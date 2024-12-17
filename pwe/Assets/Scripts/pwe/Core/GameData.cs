using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Core
{
    [Serializable]
    public class GameData
    {
        public string name;
        public enum GAMES
        {
            COOKING,
            PHOTOS,
            ABC,
            COMPARE_CONTRAST
        }
        public GameObject gameGO;
        public LevelData level;

        public List<LevelData> levels;
        public GAMES game;

        [Serializable]
        public class LevelData
        {
            public string levelName = "main";
            public int level;
        }
        public LevelData LevelMain
        {
            get
            {
                foreach (LevelData levelData in levels)
                {
                    if (levelData.levelName == "main") return levelData;
                }
                return null;
            }
        }
        public LevelData GetLevelData(string levelName)
        {
            foreach (LevelData levelData in levels)
            {
                if (levelData.levelName == levelName)
                {
                    level = levelData;
                    return levelData;
                }
            }
            return null;
        }
        LevelData AddLevel(GAMES game, string levelName)
        {
            LevelData l = new LevelData();
            l.levelName = levelName;
            levels.Add(l);
            return l;
        }
        public void LevelUp()
        {
            Debug.Log("LevelUp " + game + " level: " + level.levelName);
            level.level++;
            PlayerPrefs.SetInt(game.ToString() + "_" + level.levelName, level.level);
            Events.GameLeveled(game, level.level);
        }
        public void Init()
        {
            foreach(LevelData levelData in levels)
            {
                levelData.level = PlayerPrefs.GetInt(game.ToString() + "_" + levelData.levelName, 0);
            }           
        }
        public void Reset()
        {
            foreach (LevelData levelData in levels)
            {
                levelData.level = 0;
                PlayerPrefs.SetInt(game.ToString() + "_" + levelData.levelName, 0);
            }
        }
    }
}