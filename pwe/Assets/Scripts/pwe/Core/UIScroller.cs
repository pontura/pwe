using UnityEngine;

namespace Pwe.Core
{
    public class UIScroller : MonoBehaviour
    {
        [SerializeField] Transform container;
        [SerializeField] float speed =  0.03f;
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
            float _y = Mathf.Lerp(container.transform.localPosition.y, dest_y, speed);
            container.transform.localPosition = new Vector2(container.transform.localPosition.x, _y);
        }
        public void Scroll(bool top)
        {
            if (top) id++; else id--;
            if (id > total-1) id = total-1;
            else if (id < 0) id = 0;
            print("scroll " + top + "id: " + id);
        }
    }
}
