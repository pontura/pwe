using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pwe.Games.Abc
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] ItemsManager itemsManager;
        [SerializeField] Transform container;
        [field: SerializeField] public List<LevelStep> LevelSteps { get; private set; }
        [field: SerializeField] public int CurrentStep { get; private set; }

        public void InitLevel() {
            foreach (Transform child in container)
                Destroy(child.gameObject);
            LevelStep ls = LevelSteps[CurrentStep];
            GameObject itemsContainer = Instantiate(ls.ItemContainerPrefab,container);
            if (ls.LevelType == LevelType.REMOVE)
                itemsManager.SetItemsToRemove(ls.ItemPrefab, itemsContainer.transform);
            else
                itemsManager.SetItemsToShow(ls.ItemPrefab, itemsContainer.transform);
            Instantiate(ls.ItemToDragPrefab, container);
        }

    }
}