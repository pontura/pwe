using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs;
using System;

namespace Pwe.Games.SolarSystem
{
    public class CamClickInput : InputManager
    {
        public event Action<Vector2> OnClickInput;
        public override void OnInitPress(Vector2 pos)
        {
            OnClickInput(pos);
        }

        private void OnDestroy() {
            OnClickInput = null;
        }
    }
}
