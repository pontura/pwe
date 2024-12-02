using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPathElement : Yaguar.Inputs2D.DragElement
{
    Vector3 _lastFrameMousePos;

    public Pwe.Games.Common.LinesPath linePath;
        
    public override void InitDrag(Vector3 pos) {
        if (state == states.DRAGGING) return;
        OnInitDrag();        
        state = states.DRAGGING;
        _lastFrameMousePos = pos;
    }
    public override void Move(Vector2 pos) {
        if (state == states.IDLE) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = linePath.GetPosition(_lastFrameMousePos, worldPos);
        _lastFrameMousePos = worldPos;
    }
}
