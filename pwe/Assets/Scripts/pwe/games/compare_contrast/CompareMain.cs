using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Compare
{
    public class CompareMain : Game
    {
        [field: SerializeField] public CompareData data { get; private set; }
    }
}
