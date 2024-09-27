using UnityEngine;

namespace Yaguar.Inputs
{
    public class DragElement : InteractiveElement
    {
        states state;
        Vector2 offset;

        enum states
        {
            IDLE,
            DRAGGING
        }

        BoxCollider coll;
        InteractiveElement ie;
        void Start()
        {
            coll = GetComponent<BoxCollider>();
            coll.isTrigger = true;
            ie = GetComponent<InteractiveElement>();
        }

        public void InitDrag(Vector2 pos)
        {
            if (state == states.DRAGGING) return;
            Vector3 worldPos = GetWorldPos(pos);
            offset = transform.position - worldPos;
            state = states.DRAGGING;
        }
        public void Move(Vector2 pos)
        {
            if (state == states.IDLE) return;
            Vector3 worldPos = GetWorldPos(pos);
            worldPos += new Vector3(offset.x, offset.y, transform.position.z);
            transform.position = worldPos;
        }
        public void EndDrag()
        {
            if (state == states.IDLE) return;
            state = states.IDLE;
        }
        Vector3 GetWorldPos(Vector2 pos)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            worldPos.z = transform.position.z;
            return worldPos;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (state != states.DRAGGING) return;
            InteractiveElement ie = other.GetComponent<InteractiveElement>();
            if (ie != null)
                ie.OnIECollisionEnter(ie);
        }
        private void OnTriggerExit(Collider other)
        {
            if (state != states.DRAGGING) return;
            InteractiveElement ie = other.GetComponent<InteractiveElement>();
            if (ie != null)
                ie.OnIECollisionExit(ie);
        }
    }
}
