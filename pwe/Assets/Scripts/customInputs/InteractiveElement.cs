using UnityEngine;
using UnityEditor;

namespace Yaguar.Inputs
{
    [RequireComponent(typeof(Collider))]
    public class InteractiveElement : MonoBehaviour
    {
        public int id;
        public void Init(int id)
        {
            this.id = id;
            OnInit();
        }
        public virtual void OnInit() { }
        public virtual void OnIECollisionEnter(InteractiveElement ie)  { Debug.Log("OnCollision Enter: " + ie.name); }
        public virtual void OnIECollisionExit(InteractiveElement ie)  { Debug.Log("OnCollision Exit: " + ie.name); }

    }
}