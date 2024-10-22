using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YaguarLib.Xtras
{
    public class Screenshot : MonoBehaviour
    {
        public Vector2Int shotRes;
        public Vector2 shotCenter;
        public Camera cameraToScreen;
        private bool takeShot = false;
        private bool copyTex = false;
        private Texture2D texture;
        private System.Action<Texture2D> CopyTexture;

        // Start is called before the first frame update
        void Start() {
            if (cameraToScreen == null)
                cameraToScreen = GetComponent<Camera>();
        }

        public void TakeShot(System.Action<Texture2D> copytex) {
            takeShot = true;
            // Debug.Log("TAKE SHOT");
            CopyTexture = copytex;
        }        

        public void TakeShot(Vector2 center, System.Action<Texture2D> copytex) {
            shotCenter = center;
            takeShot = true;
            // Debug.Log("TAKE SHOT");
            CopyTexture = copytex;
        }

        void LateUpdate() {

            if (copyTex) {
                CopyTexture(texture);
                copyTex = false;
            }

            if (takeShot) {
                //Debug.Log("Screen: "+Screen.width + " x " + Screen.height);
                //Debug.Log("Center: " + shotCenter.x + " x " + shotCenter.y);
                RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 32);
                texture = new Texture2D(shotRes.x, shotRes.y, TextureFormat.RGB24, false);                

                cameraToScreen.targetTexture = rt;

                cameraToScreen.Render();
                RenderTexture.active = rt;
                //Debug.Log("# " + (int)(shotCenter.x - (0.5f * shotRes.x)) + " x " + (int)(shotCenter.y - (0.5f * shotRes.y)));
                //Debug.Log("# " + (Screen.height - (int)(shotCenter.y + (0.5f * shotRes.y))));
                //texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), (int) (shotCenter.x-(0.5f*shotRes.x)), (int) (shotCenter.y - (0.5f * shotRes.y)));
                Rect r = new Rect();
                r.xMin = System.Math.Max(0, (int)(shotCenter.x - (0.5f * shotRes.x)));
                r.yMin = System.Math.Max(0, (int)(Screen.height - (shotCenter.y + (0.5f * shotRes.y))));
                r.xMax = System.Math.Min(Screen.width, (int)(shotCenter.x + (0.5f * shotRes.x)));
                r.yMax = System.Math.Min(Screen.height, (int)(Screen.height - (shotCenter.y - (0.5f * shotRes.y))));                
                texture.ReadPixels(r, 0, 0);

                texture.Apply();

                cameraToScreen.targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                Destroy(rt);

                takeShot = false;
                copyTex = true;
            }
        }
    }
}