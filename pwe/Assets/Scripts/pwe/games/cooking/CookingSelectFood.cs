using Pwe.Games.Cooking.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.UI;
using static ItemData;

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