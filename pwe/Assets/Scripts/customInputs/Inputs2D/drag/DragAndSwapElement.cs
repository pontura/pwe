using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;

namespace Yaguar.Inputs2D
{
    public class DragAndSwapElement : DragElement
    {
        DragAndSwapElement otherInteractiveElement;
        Vector3 originalPos = Vector2.zero;
        public Vector3 OriginalPos { get { return originalPos; } }

        public override void OnStart()
        {
            base.OnStart();
            originalPos = transform.position;
        }
        public void SetInteractiveElementToSwap(DragAndSwapElement ie)
        {
            otherInteractiveElement = ie;
            print(gameObject.name + "_________SetInteractiveElementToSwap " + otherInteractiveElement.gameObject.name);
        }
        public override void OnEndDrag()
        {
            print(gameObject.name +  "_________OnEndDrag ____otherInteractiveElement " + otherInteractiveElement);
            if(otherInteractiveElement != null)
            {
                Vector3 myPos = OriginalPos;
                Repositionate(otherInteractiveElement.OriginalPos);
                otherInteractiveElement.Repositionate(myPos);
            }
            else
            {
                transform.position = OriginalPos;
            }
            otherInteractiveElement = null;
        }

        public override void OnIECollisionEnter(InteractiveElement ie)
        {
            print(gameObject.name + " _________ " + ie.gameObject.name);
            if (ie.GetComponent<DragAndSwapElement>() != null)
            {
                ie.GetComponent<DragAndSwapElement>().SetInteractiveElementToSwap(this);
            }
        }
        public override void OnIECollisionExit(InteractiveElement ie)
        {
            if (ie.GetComponent<DragAndSwapElement>() != null)
            {
                print("_________ OnIECollisionExit " + ie);
                if (otherInteractiveElement != null && otherInteractiveElement == ie.GetComponent<DragAndSwapElement>())
                {
                    otherInteractiveElement = null;
                }
            }
        }
        public void Repositionate(Vector3 pos)
        {
            transform.position = pos;
            originalPos = pos;
        }

    }
}
