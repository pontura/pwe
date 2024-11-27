using UnityEngine;
namespace YaguarLib.Xtras
{
    public class ResolutionFixer : MonoBehaviour
    {
        [SerializeField] Vector2 ipad_pos;
        [SerializeField] float ipad_scale;
        float aspect = 0;
        Vector2 originalPos = Vector2.zero;
        Vector2 oringialScale = Vector2.one;

        public void Rescaled()
        {
            originalPos = transform.localPosition;
            oringialScale = transform.localScale;
            Recalculate();
        }
        private void OnEnable()
        {
            if (originalPos == Vector2.zero)
            {
                originalPos = transform.localPosition;
                oringialScale = transform.localScale;
            }
            Loop();
        }
        private void OnDisable()
        {
            CancelInvoke();
        }
        private void Loop()
        {
            float newAspect = (float)Screen.width / (float)Screen.height;
            if (newAspect != aspect)
            {
                aspect = newAspect;
                Recalculate();
            }
            Invoke("Loop", 1);
        }
        void Recalculate()
        {
            RecalculateByDevice(aspect <= 1.34);
        }
        void RecalculateByDevice(bool isIpad)
        {
            if(isIpad)
            {
                transform.localPosition = ipad_pos;
                float _y = 1;
                if (transform.localScale.y < 0) _y = -1;
                transform.localScale= new Vector2(ipad_scale, ipad_scale * _y);
            }
            else
            {
                transform.localPosition = originalPos;
                transform.localScale = oringialScale;
            }
        }

    }
}
