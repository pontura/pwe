using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToChange : InteractiveElement
    {
        protected System.Action OnChange;
         
        public virtual void Init(System.Action onChange = null) {
            OnChange = onChange;
        }
        public override void OnIECollisionEnter(InteractiveElement ie) {
            Debug.Log("#OnIECollisionEnter: " + gameObject.name);
            if(OnChange != null)
                OnChange();            
        }
    }

}
