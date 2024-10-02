using System;
using UnityEngine;

namespace Core
{
    public class Swapper : MonoBehaviour
    {
        public Colorizable colorCombos;
        [Serializable] public class Colorizable
        {
            public ColorCombos[] colorCombo;
            public SpritesCombos[] spritesForCombos;
            public void SetRandom()
            {
                ColorCombos c = colorCombo[UnityEngine.Random.Range(0, colorCombo.Length)];
                int id = 0;
                foreach (Color color in c.color)
                {
                    if (spritesForCombos.Length > id)
                    {
                        SpritesCombos s = spritesForCombos[id];
                        foreach (SpriteRenderer sr in s.sprites)
                            sr.color = color;
                        id++;
                    }
                }
            }
        }
        [Serializable]
        public class ColorCombos
        {
            public Color[] color;
        }
        [Serializable]
        public class SpritesCombos
        {
            public SpriteRenderer[] sprites;
        }
        [SerializeField] GameObject[] randomGameobjects;

        [SerializeField] Color[] randomColors;
        [SerializeField] SpriteRenderer[] spritesToColorize;

        void OnEnable()
        {
            if (randomGameobjects != null && randomGameobjects.Length > 0)
                SetRanbomGO();
            if (randomColors != null && randomColors.Length > 0)
                SetRandomColor();
            if(colorCombos.colorCombo.Length>0)
                colorCombos.SetRandom();
        }
        void SetRanbomGO()
        {
            foreach (GameObject go in randomGameobjects)
                go.SetActive(false);
            randomGameobjects[UnityEngine.Random.Range(0, randomGameobjects.Length)].SetActive(true);
        }
        void SetRandomColor()
        {
            Color color = randomColors[UnityEngine.Random.Range(0, randomColors.Length)];
            if (spritesToColorize == null || spritesToColorize.Length == 0)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = color;
            }
            else
            {
                foreach (SpriteRenderer s in spritesToColorize)
                    s.color = color;
            }
        }
    }
}
