using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    public class PlanetItemUI : MonoBehaviour
    {
        [SerializeField] Image bg;
        [SerializeField] Image image;
        [SerializeField] GameObject done;
        [SerializeField] Image frame;
        [field:SerializeField] public PlanetName Planet_Name { get; private set; }        

        public void Init(PlanetData data, Color bgColor)
        {
            Planet_Name = data.planetName;
            image.sprite = data.sprite;
            bg.color = bgColor;
        }

        public void SetDone() {
            done.SetActive(true);
        }

        public void SetImage(Texture2D tex) {
            if (tex == null)
                return;
            transform.localScale = 0.35f * Vector3.one;
            frame.enabled = true;
            image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }

    }
}