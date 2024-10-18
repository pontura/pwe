using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;
using System;
using Yaguar.Inputs2D;

namespace Pwe.Games.SolarSystem
{
    public class CamClickInput : InputManager
    {
        public event Action<Vector2> OnClickInput;
        public override void OnInitPress(Vector2 pos)
        {
            OnClickInput(pos);
            Check();
        }

        private void OnDestroy() {
            OnClickInput = null;
        }

        void Check() {
            Debug.Log("# ChECK");
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Debug.Log("# hits length: "+hits.Length);
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider != null) {
                    Debug.Log("# HIT COLLIDER");
                    InteractiveElement ie = hit.collider.transform.parent.parent.gameObject.GetComponent<InteractiveElement>();
                    Debug.Log("# ie="+(ie==null));
                    if (ie == null) continue;
                    ie.OnClicked();
                }
            }
        }
    }
}
