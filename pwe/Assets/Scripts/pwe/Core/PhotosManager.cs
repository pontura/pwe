using UnityEngine;
using System;
using System.IO;

namespace Pwe.Core
{
    public class PhotosManager : MonoBehaviour
    {
        [SerializeField] string photosFolder;

        public void SavePhoto(string filename, Texture2D tex) {
            byte[] bytes = tex.EncodeToPNG();
            string folder = Path.Combine(Application.persistentDataPath, photosFolder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, filename);
            System.IO.File.WriteAllBytes(path, bytes);
            Debug.Log(string.Format("thumb to: {0}", path));
        }

        public Texture2D LoadPhoto(string filename) {
            string folder = Path.Combine(Application.persistentDataPath, photosFolder);
            string path = Path.Combine(folder, filename);
            if (System.IO.File.Exists(path)) {
                var bytes = System.IO.File.ReadAllBytes(path);
                Texture2D texture2d = new Texture2D(1, 1);
                texture2d.LoadImage(bytes);
                return texture2d;
            } else {
                Debug.Log("FILE NOT FOUND AT: " + path);
                return null;
            }
        }

    }
}
