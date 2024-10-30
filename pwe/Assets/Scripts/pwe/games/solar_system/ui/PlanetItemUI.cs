using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    public class PlanetItemUI : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] GameObject done;
        [field:SerializeField] public PlanetName Planet_Name { get; private set; }        

        public void Init(PlanetData data)
        {
            Planet_Name = data.planetName;
            image.sprite = data.sprite;
        }

        public void SetDone() {
            done.SetActive(true);
        }

        public void SetImage(Texture2D tex) {
            if (tex == null)
                return;
            image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }

    }
}