using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class PlanetItemUI : MonoBehaviour
    {
        [SerializeField] Image bg;
        [SerializeField] Image image;
        [SerializeField] GameObject done;
        [SerializeField] Image frame;
        [field:SerializeField] public PlanetName Planet_Name { get; private set; }

        Button _button;
        Animator _anim;

        System.Action _onButtonSelected;

        [SerializeField] PlanetState planetState;
        public enum PlanetState
        {
            done,
            undone,
            normal,
            won
        }

        public void Init(PlanetData data, PlanetState state, System.Action onClick = null) {
            SetButton(onClick);
            Planet_Name = data.planetName;
            image.sprite = data.sprite;
            image.color = data.color;
            planetState = state;
            _anim = GetComponent<Animator>();
            _anim.Play("entry");
            if (planetState != PlanetState.normal)
                Invoke(nameof(SetPlanetExit), 1);
        }

        void SetPlanetExit() {
            _anim.Play("exit");
        }

        public void UpdatePlanetSate() {
            Debug.Log("#UpdatePlanetSate: " + planetState.ToString());
            _anim.Play(planetState.ToString());
            if(_buttonizeCall!=null)
                _button.onClick.RemoveListener(_buttonizeCall);
        }

        public void SetDone() {
            done.SetActive(true);
        }

        public void SetImage(PlanetData pd) {
            _anim = GetComponent<Animator>();
            if (pd.lastPhoto == null) {
                _anim.Play("off");
                return;
            }
            transform.localScale = 0.35f * Vector3.one;
            //frame.enabled = true;
            image.sprite = Sprite.Create(pd.lastPhoto, new Rect(0, 0, pd.lastPhoto.width, pd.lastPhoto.height), Vector2.zero);
            if (pd.hasNewPhoto) {
                pd.hasNewPhoto = false;
                _anim.Play("new");
            } else
                _anim.Play("on");
        }

        UnityEngine.Events.UnityAction _buttonizeCall = null;
        public void SetAsButton(PlanetName planetName, System.Action callback) {
            _anim.Play("buttonize");
            string animName = planetName == Planet_Name ? "button_right" : "button_wrong";
            _buttonizeCall = () => {
                _anim.Play(animName);
                if (planetName == Planet_Name) {                    
                    planetState = PlanetState.won;
                    callback();
                }
            };
            _button.onClick.AddListener(_buttonizeCall);
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