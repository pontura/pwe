using Pwe.Games.Cooking;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outscene : Cutscene
{
    [SerializeField] SlicerCreator slicer;

    public override void OnInit()
    {
        base.OnInit();
        slicer.Init();
    }
}
