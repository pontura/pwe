using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToRemove : ItemToChange
    {        
        public override void OnIECollisionEnter(InteractiveElement ie) {
            base.OnIECollisionEnter(ie);
            Destroy(gameObject);
        }
    }

}
