using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToRemove : InteractiveElement
    {
        System.Action OnRemove;
         
        public void Init(System.Action onRemove) {
            OnRemove = onRemove;
        }
        public override void OnIECollisionEnter(InteractiveElement ie) {
            if(OnRemove!=null)
                OnRemove();
            Destroy(gameObject);
        }
    }

}
