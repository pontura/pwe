using UnityEngine;

namespace Yaguar.Inputs2D
{
    public class DragElement : InteractiveElement {
        
        states state;
        Vector3 offset;

        protected enum states
        {
            IDLE,
            DRAGGING
        }

        BoxCollider2D coll;
        InteractiveElement ie;
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            coll.isTrigger = true;
            ie = GetComponent<InteractiveElement>();
        }
        public virtual void ForcePosition(Vector3 pos)
        {
            transform.position = GetWorldPos(pos);
        }
        public virtual void InitDrag(Vector3 pos)
        {
            if (state == states.DRAGGING) return;
            OnInitDrag();
            Vector3 worldPos = GetWorldPos(pos);
            offset = transform.position - worldPos;
            state = states.DRAGGING;
        }
        public virtual void Move(Vector2 pos)
        {
            if (state == states.IDLE) return;
            Vector3 worldPos = GetWorldPos(pos);
            transform.position = new Vector3(offset.x+worldPos.x, offset.y+worldPos.y, transform.position.z);
        }
        public void EndDrag()
        {
            if (state == states.IDLE) return;
            OnEndDrag();
            state = states.IDLE;
        }
        Vector3 GetWorldPos(Vector2 pos)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            return worldPos;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("OnTriggerEnter2D" + other);
            if (state != states.DRAGGING) return;
            InteractiveElement ie = other.GetComponent<InteractiveElement>();
            if (ie != null)
                ie.OnIECollisionEnter(ie);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            print("OnTriggerExit2D" + other);
            if (state != states.DRAGGING) return;
            InteractiveElement ie = other.GetComponent<InteractiveElement>();
            if (ie != null)
                ie.OnIECollisionExit(ie);
        }
        public virtual void OnEndDrag() { }
        public virtual void OnInitDrag() { }
    }
}
