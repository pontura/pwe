using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pwe.Games.Common;

namespace Pwe.Games.Abc
{
    [Serializable]
    public class LevelStep {
        [field: SerializeField] public GameObject ItemContainerPrefab { get; private set; }
        [field: SerializeField] public GameObject ItemToDragPrefab { get; private set; }
        [field: SerializeField] public Transform ItemToDragPosition { get; private set; }

        [field: SerializeField] public ItemToInteractWith ItemPrefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public LinesPath path { get; private set; }
    }
}