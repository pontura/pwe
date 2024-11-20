using UnityEngine;

namespace Yaguar.Inputs2D
{
    public class DragElementUI : DragElement{
        
        states state;
        Vector2 offset;              

        public override void ForcePosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public override void InitDrag(Vector3 pos)
        {
            if (state == states.DRAGGING) return;
            OnInitDrag();
            offset = transform.position - pos;
            state = states.DRAGGING;
        }
        public override void Move(Vector2 pos)
        {
            if (state == states.IDLE) return;
            transform.position = pos + offset;
        }
        
    }
}
