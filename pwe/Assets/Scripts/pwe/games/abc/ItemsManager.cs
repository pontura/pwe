using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pwe.Games.Abc
{
    public class ItemsManager : MonoBehaviour {

        [SerializeField] Transform container;
        [SerializeField] Yaguar.Inputs2D.InteractiveElement item_prefab;

        List<GameObject> allItems;

        // Start is called before the first frame update
        void Start () {
            //SetItemsToRemove();            
            //SetItemsToShow(item_prefab, container);
        }

        public void SetItemsToRemove(Yaguar.Inputs2D.InteractiveElement itemPrefab, Transform container) {
            allItems = new();
            foreach (Transform t in container.GetComponentsInChildren<Transform>()) {
                if (t == container)
                    continue;
                Debug.Log("#" + t.gameObject.name);
                ItemToRemove item = Instantiate(itemPrefab, container) as ItemToRemove;
                allItems.Add(item.gameObject);
                item.transform.localPosition = t.localPosition;
                t.gameObject.SetActive(false);
            }
        }

        public void SetItemsToShow(Yaguar.Inputs2D.InteractiveElement itemPrefab, Transform container) {
            allItems = new();
            foreach (Transform t in container.GetComponentsInChildren<Transform>()) {
                if (t == container)
                    continue;
                Debug.Log("#" + t.gameObject.name);
                ItemToShow item = Instantiate(itemPrefab, container) as ItemToShow;
                allItems.Add(item.gameObject);
                item.transform.localPosition = t.localPosition;
                item.Init(null);
                t.gameObject.SetActive(false);
            }
        }

        public void RemoveItem(GameObject item) {
            allItems.Remove(item);
            Destroy(item);
        }
    }

}
