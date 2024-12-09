using Pwe.Games.Cooking;
using Rive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outscene : Cutscene
{

    [SerializeField] CookingMainPiece mainPiece;
    [SerializeField] Transform mainPieceContainer;

    public override void OnInit()
    {
        base.OnInit();
        mainPiece.transform.parent = mainPieceContainer; 
        mainPiece.transform.localScale = Vector3.one;
        mainPiece.transform.localPosition = Vector3.zero;

    }
}
