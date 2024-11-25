using UnityEngine;
using UnityEditor;

namespace Yaguar.Inputs2D
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractiveElement : MonoBehaviour
    {
        [HideInInspector] public int id;
        private void Start()
        {
            OnStart();
        }
        public void Init(int id)
        {
            this.id = id;
            OnInit();
        }
        public virtual void OnStart() { }
        public virtual void OnInit() { }
        public virtual void OnIECollisionEnter(InteractiveElement ie)  { Debug.Log("OnCollision Enter: " + ie.name); }
        public virtual void OnIECollisionExit(InteractiveElement ie)  { Debug.Log("OnCollision Exit: " + ie.name); }
        public virtual void OnClicked()  { Debug.Log("OnClicked" + gameObject.name); }

    }
}