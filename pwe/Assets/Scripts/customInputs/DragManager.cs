using System.Collections.Generic;
using UnityEngine;

namespace Yaguar.Inputs
{
    public class DragInputManager : InputManager
    {
        public List<DragElement> elements;
        DragElement dragElement;

        public override void OnInit()
        {
            base.OnInit();
        }
        public void AddElement(DragElement e)
        {
            elements.Add(e);
            e.Init(elements.Count);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
                return hit.collider.gameObject.GetComponent<DragElement>();
            }
            return null;
        }
    }
}
