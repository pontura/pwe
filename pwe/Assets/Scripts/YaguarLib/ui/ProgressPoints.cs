using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;

namespace YaguarLib.UI
{
    public class ProgressPoints : MonoBehaviour
    {
        [SerializeField] Animator pointImageGO_to_add;
        [SerializeField] List<GameObject> all;
        [SerializeField] Transform container;
        [SerializeField] AnimationClip idleClip;
        [SerializeField] AnimationClip selectedClip;
        public types type;
        public enum types {
            single,
            additive
        }

        int selected;
        int totalPoints;

        public void Init(int totalPoints, int selected, types type, System.Action<int> OnClicked = null)
        {
            this.type = type;
            this.totalPoints = totalPoints;
            this.selected = selected;

            all = new List<GameObject>();
            Utils.RemoveAllChildsIn(container);

            
            for (int a = 0; a < totalPoints; a++)
            {
                GameObject i = Instantiate(pointImageGO_to_add, container).gameObject;
                all.Add(i);
                ButtonUI b = i.GetComponent<ButtonUI>();
                if (b != null) b.Init(OnClicked, a);
            }

            SetValue(selected);
        }
        public void SetValue(int selected)
        {           
            if (selected >= 0 && selected < all.Count)
            {
                if (type == types.additive)
                {
                    int id = 0;
                    foreach (GameObject go in all)
                    {
                        if (id <= selected)
                            go.GetComponent<Animator>().Play(selectedClip.name);
                        else
                            go.GetComponent<Animator>().Play(idleClip.name);
                        id++;
                    }
                } else
                {
                    Reset();
                    all[selected].GetComponent<Animator>().Play(selectedClip.name);
                }
            }
        }
        private void Reset()
        {
            foreach (GameObject a in all)
                a.GetComponent<Animator>().Play(idleClip.name);
        }
    }
}