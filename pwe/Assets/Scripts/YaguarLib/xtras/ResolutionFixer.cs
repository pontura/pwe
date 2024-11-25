using PlasticGui;
using UnityEngine;
namespace YaguarLib.Xtras
{
    [ExecuteAlways]
    public class ResolutionFixer : MonoBehaviour
    {
        [SerializeField] Vector2 ipad_pos;
        [SerializeField] float ipad_scale;
        float aspect = 0;
        Vector2 originalPos = Vector2.zero;
        Vector2 oringialScale = Vector2.one;

        private void Start()
        {
            aspect = (float)Screen.width / (float)Screen.height;
        }
        private void OnEnable()
        {
            Invoke("Delayed", 0.1f);
        }
        void Delayed()
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
            print("Loop for IPAD: " + (aspect <= 1.34f) + " aspect: " + aspect);
            Invoke("Loop", 1);
        }
        void Recalculate()
        {
            print("Recalculate for IPAD: " + (aspect <= 1.34f) + " aspect: " + aspect);
            RecalculateByDevice(aspect <= 1.34);
        }
        void RecalculateByDevice(bool isIpad)
        {
            if(isIpad)
            {
                transform.localPosition = ipad_pos;
                transform.localScale= oringialScale * ipad_scale;
            }
            else
            {
                transform.localPosition = originalPos;
                transform.localScale = oringialScale;
            }
        }

    }
}
