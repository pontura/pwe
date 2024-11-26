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
        gummybears
    }
    public Items item;
    public int num;
}
