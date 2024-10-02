using System.Collections.Generic;
using UnityEngine;

namespace Yaguar.Inputs
{
    public class DragInputManager : InputManager
    {
        DragElement dragElement;

        public void ForceDrag(Vector2 screenPos, DragElement dragElement)
        {
            this.dragElement = dragElement;
            dragElement.ForcePosition(screenPos);
            dragElement.InitDrag(screenPos);
        }
        public override void OnInitPress(Vector2 pos)
        {
            dragElement = CheckDrag();
            if (dragElement == null) return;
            dragElement.InitDrag(pos);
        }
        public override void OnRelease(Vector2 pos)
        {
            if (dragElement == null) return;
            dragElement.EndDrag();
            dragElement = null;
        }
        public override void OnMove(Vector2 pos)
        {
            if (dragElement == null) return;
            dragElement.Move(pos);
        }
        DragElement CheckDrag()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                return hit.collider.gameObject.GetComponent<DragElement>();

            return null;
        }
    }
}
