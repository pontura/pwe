using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.UI;
namespace Pwe.Games.Cooking
{
    public class CookingSelectFood : GameMain
    {
        [SerializeField] ButtonUI[] buttons;
        void Start()
        {
            foreach (ButtonUI b in buttons)
            {
                b.Init(Clicked);
            }
        }
        void Clicked()
        {
            Next();
        }
    }

}