using Pwe.Games.Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicerCreator : MonoBehaviour
{

    [SerializeField] CookingMainPiece piece;
    [SerializeField] Transform[] mainPieceContainer;

    public void Init()
    {
        foreach (Transform t in mainPieceContainer)
        {
            CookingMainPiece newPiece = Instantiate(piece, t);
            newPiece.transform.localScale = Vector3.one;
            newPiece.transform.localPosition = Vector3.zero;
            newPiece.transform.eulerAngles = Vector3.zero;
            foreach (Image i in newPiece.GetComponentsInChildren<Image>())
            {
                i.maskable = true;
            }
        }

    }
}
