using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;

namespace Yaguar.Inputs2D
{
    public class ClickInputManager : InputManager
    {
        public override void OnInitPress(Vector2 pos)
        {
            InteractiveElement ie = Check();
            if (ie == null) return;
            ie.OnClicked();
        }        
        InteractiveElement Check()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                return hit.collider.gameObject.GetComponent<InteractiveElement>();

            return null;
        }
    }
}
