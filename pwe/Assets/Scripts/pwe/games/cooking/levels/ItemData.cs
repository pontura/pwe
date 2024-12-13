using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemData
{
    public enum Items
    {
        tomatoes,
        olives,
        gummybears,
        onions,
        eggs,
        avocados,
        almonds,
        mushrooms,
        salamis,
        bellpeppers,
        cheesecubes,
        bubbles,
        chickennuggets,
        bonecookies,
        beans,
        chocolatehearts,
        lettuces,
        atoms,
        oranges
    }
    public Items item;
    public int num;
}
