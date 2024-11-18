using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pwe.Games.Cooking
{
    public class CookingGame : Game
    {        
        [field:SerializeField] public CookingData CookingData { get; private set; }        
    }
}