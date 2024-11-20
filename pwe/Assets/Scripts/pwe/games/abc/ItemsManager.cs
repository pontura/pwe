using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pwe.Games.Abc
{
    public class ItemsManager : MonoBehaviour
    {
        [SerializeField] ItemToRemove itemToRemove_prefab;
        [SerializeField] ItemToShow itemToShow_prefab;

        List<GameObject> allItems;

        // Start is called before the first frame update
        void Start () {
            //SetItemsToRemove();            
            SetItemsToShow();
        }

        public void SetItemsToRemove() {
            allItems = new();
            foreach (Transform t in GetComponentsInChildren<Transform>()) {
                if (t == transform)
                    continue;
                Debug.Log("#" + t.gameObject.name);
                ItemToRemove item = Instantiate(itemToRemove_prefab, transform);
                allItems.Add(item.gameObject);
                item.transform.localPosition = t.localPosition;
                t.gameObject.SetActive(false);
            }
        }

        public void SetItemsToShow() {
            allItems = new();
            foreach (Transform t in GetComponentsInChildren<Transform>()) {
                if (t == transform)
                    continue;
                Debug.Log("#" + t.gameObject.name);
                ItemToShow item = Instantiate(itemToShow_prefab, transform);
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
