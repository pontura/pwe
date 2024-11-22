using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToShow : ItemToChange
    {
        [SerializeField] SpriteRenderer sr;
         
        public override void Init(System.Action onChange) {
            base.Init(onChange);
            sr.enabled = false;
        }
        public override void OnIECollisionEnter(InteractiveElement ie) {
            base.OnIECollisionEnter(ie);
            sr.enabled = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
