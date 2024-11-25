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

        public event System.Action OnLevelCompleted;

        private void Start() {
            itemsManager.OnStepCompleted += OnStepCompleted;
        }

        private void OnDestroy() {
            OnLevelCompleted = null;
            itemsManager.OnStepCompleted -= OnStepCompleted;
        }

        public void InitLevel() {
            foreach (Transform child in container)
                Destroy(child.gameObject);
            LevelStep ls = LevelSteps[CurrentStep];
            GameObject itemsContainer = Instantiate(ls.ItemContainerPrefab,container);
            itemsManager.SetItems(ls.ItemPrefab, itemsContainer.transform);            
            GameObject drag = Instantiate(ls.ItemToDragPrefab, container);
            drag.transform.localPosition = ls.ItemToDragPosition.position;
            if (ls.path != null) {
                drag.GetComponent<DragPathElement>().linePath = ls.path;
            }

        }

        void OnStepCompleted() {
            CurrentStep++;
            if (CurrentStep >= LevelSteps.Count) {
                OnLevelCompleted();
                return;
            }
            InitLevel();
        }

    }
}