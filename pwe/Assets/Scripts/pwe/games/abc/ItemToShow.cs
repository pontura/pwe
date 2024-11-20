using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Abc
{
    public class ItemToShow : InteractiveElement
    {
        [SerializeField] SpriteRenderer sr;
        System.Action OnShow;
         
        public void Init(System.Action onShow) {
            OnShow = onShow;
            sr.enabled = false;
        }
        public override void OnIECollisionEnter(InteractiveElement ie) {
            Debug.Log("#OnIECollisionEnter: " + gameObject.name);
            if(OnShow!=null)
                OnShow();
            sr.enabled = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
