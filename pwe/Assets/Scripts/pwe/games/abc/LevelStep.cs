using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pwe.Games.Abc
{
    public enum LevelType
    {
        REMOVE,
        SHOW
    }

    [Serializable]
    public class LevelStep {
        [field: SerializeField] public GameObject ItemContainerPrefab { get; private set; }
        [field: SerializeField] public GameObject ItemToDragPrefab { get; private set; }
        [field: SerializeField] public Transform ItemToDragPosition { get; private set; }

        [field: SerializeField] public LevelType LevelType { get;private set; }
        [field: SerializeField] public Yaguar.Inputs2D.InteractiveElement ItemPrefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}