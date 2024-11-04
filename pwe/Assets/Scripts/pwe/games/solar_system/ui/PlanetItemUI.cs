using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    [RequireComponent(typeof(Button))]
    public class PlanetItemUI : MonoBehaviour
    {
        [SerializeField] Image bg;
        [SerializeField] Image image;
        [SerializeField] GameObject done;
        [SerializeField] Image frame;
        [field:SerializeField] public PlanetName Planet_Name { get; private set; }

        Button _button;
        public void Init(PlanetData data, Color bgColor, System.Action onClick=null)
        {
            SetButton(onClick);
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

        public void SetButton(System.Action onClick) {
            if (onClick != null) {
                if (_button == null)
                    _button = GetComponent<Button>();
                else
                    _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => onClick());
            }
        }

    }
}