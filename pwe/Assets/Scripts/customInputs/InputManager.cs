using UnityEngine;

namespace Yaguar.Inputs
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] bool isOn;
        Vector2 lastPosition;

        private void Start()
        {
            isOn = true;
        }

        protected void Init()  {
            OnInit();
        }
        void Update()
        {
            if (!isOn) return;
            CheckTouches();
        }
        
        public void SetOn(bool isOn)
        {
            this.isOn = isOn;
        }
        public virtual void OnUpdate() { }
        public virtual void OnInit() { }

        public virtual void OnInitPress(Vector2 pos) { }
        public virtual void OnRelease(Vector2 pos) { }
        public virtual void OnPress(Vector2 pos) {
            if (pos != lastPosition)
                OnMove(pos);
            lastPosition = pos;                
        }
        public virtual void OnMove(Vector2 pos) { }

        void CheckTouches()
        {
            Vector2 pos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
                OnInitPress(pos);
            else if (Input.GetMouseButton(0))
                OnPress(pos);
            else if (Input.GetMouseButtonUp(0))
                OnRelease(pos);
        }
    }
}
