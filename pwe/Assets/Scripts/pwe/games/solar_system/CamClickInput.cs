using UnityEngine;
using Yaguar.Inputs;
using System;
using UnityEngine.EventSystems;

namespace Pwe.Games.SolarSystem
{
    public class CamClickInput : InputManager
    {
        public event Action<Vector2> OnClickInput;

        Vector3 _halfShotSize;

        public override void OnInitPress(Vector2 pos)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (Input.touchCount > 0)
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
            
            OnClickInput(pos);
            Check();
        }

        public void SetShotSize(Vector2 shotSize) {
            _halfShotSize = new Vector3(0.5f*shotSize.x, 0.5f * shotSize.y,0);
        }

        private void OnDestroy() {
            OnClickInput = null;
        }

        void Check() {            
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider != null) {
                    OrbitalItem oi = hit.collider.transform.parent.parent.gameObject.GetComponent<OrbitalItem>();
                    if (oi == null) continue;
                    oi.OnClicked(Camera.main.ScreenToWorldPoint(Input.mousePosition - _halfShotSize), Camera.main.ScreenToWorldPoint(Input.mousePosition + _halfShotSize));
                }
            }
        }
    }
}
