using UnityEngine;

namespace Pwe.Games
{
    public class GameMain : MonoBehaviour
    {
        Game game;
        public Game Game { get { return game; } }
        public RiveTexture GetRiveTexture() { return game.rive; }

        public bool isOn;
        public void Initialize(Game game)
        {
            gameObject.SetActive(false);
            this.game = game;
            OnInitialize();
        }
        public void Init()
        {
            isOn = true;
            gameObject.SetActive(true);
            OnInit();
        }
        public void Hide()
        {
            isOn = false;
            gameObject.SetActive(false);
            OnHide();
        }
        public void Next() { if (!isOn) return; game.Next(); isOn = false; }

        public void Back() { if (!isOn) return; game.Back(); isOn = false; }

        private void Update()  {  if (isOn) OnUpdate();  }

        public virtual void OnInitialize() { }
        public virtual void OnInit() { }
        public virtual void OnHide() { }
        public virtual void OnUpdate() { }

    }
}
