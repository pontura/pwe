using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;

namespace Yaguar.Inputs2D
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
            if (dragElement != null) return;
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    DragElement d = hit.collider.gameObject.GetComponent<DragElement>();
                    if (d != null)
                        return hit.collider.gameObject.GetComponent<DragElement>();
                }
            }

            return null;
        }
    }
}
