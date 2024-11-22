using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToModify : ItemToInteractWith
    {
        [SerializeField] GameObject initial;
        [SerializeField] GameObject final;

        public override void Init(System.Action onChange) {
            base.Init(onChange);
            initial.SetActive(true);
            final.SetActive(false);
        }
        public override void OnIECollisionEnter(InteractiveElement ie) {
            base.OnIECollisionEnter(ie);
            initial.SetActive(false);
            final.SetActive(true);
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
