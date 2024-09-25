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

    }
}