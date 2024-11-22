using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pwe.Games.Abc
{
    public class ItemsManager : MonoBehaviour {

        [SerializeField] Transform container;
        [SerializeField] ItemToInteractWith item_prefab;

        List<GameObject> allItems;

        public event System.Action OnStepCompleted;

        int _itemsCompletedCount;

        // Start is called before the first frame update
        void Start () {
            //SetItemsToRemove();            
            //SetItemsToShow(item_prefab, container);
        }

        public void SetItems(ItemToInteractWith itemPrefab, Transform container) {
            _itemsCompletedCount = 0;
            allItems = new();
            foreach (Transform t in container.GetComponentsInChildren<Transform>()) {
                if (t == container)
                    continue;
                Debug.Log("#" + t.gameObject.name);
                ItemToInteractWith item = Instantiate(itemPrefab, container);
                allItems.Add(item.gameObject);
                item.transform.localPosition = t.localPosition;
                item.Init(OnChange);
                t.gameObject.SetActive(false);
            }
        }     
        
        void OnChange() {
            _itemsCompletedCount++;
            if (_itemsCompletedCount >= allItems.Count) {
                OnStepCompleted();
            }
        }

        public void RemoveItem(GameObject item) {
            allItems.Remove(item);
            Destroy(item);
        }
    }

}
