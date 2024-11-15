using Codice.Client.BaseCommands.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Core
{
    public class UIScroller : MonoBehaviour
    {
        [SerializeField] Transform container;
        float separation_y;
        int id = 0;
        int total;
        public void Init(int id, int total, int separation_y)
        {
            this.separation_y = separation_y;
            this.id = id;
            this.total = total;
        }

        void Update()
        {
            float dest_y = id * separation_y * -1;
            float _y = Mathf.Lerp(container.transform.localPosition.y, dest_y, 0.1f);
            container.transform.localPosition = new Vector2(container.transform.localPosition.x, _y);
        }
        public void Scroll(bool top)
        {
            if (top) id++; else id--;
            if (id > total) id = total;
            else if (id < 0) id = 0;
            print("scroll " + top + "id: " + id);
        }
    }
}
