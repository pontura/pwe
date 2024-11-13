using UnityEngine;
namespace YaguarLib.Xtras
{
    [ExecuteAlways]
    public class Parallax : MonoBehaviour
    {
        [SerializeField] Transform target;
        Vector3 lastPos;
        [SerializeField] Vector2 scale = Vector2.one;

        public void SetCam(Transform cam) { target = cam; }

        private void LateUpdate() {
            if (target == null) return;
            else {
                Vector2 deltaPos = target.position - lastPos;
                transform.position += (Vector3)(deltaPos * scale);

                lastPos = target.position;
            }
        }
    }
}
