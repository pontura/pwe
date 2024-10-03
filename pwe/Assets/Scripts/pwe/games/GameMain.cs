using UnityEngine;

namespace Pwe.Games
{
    public class GameMain : MonoBehaviour
    {
        Game game;
        public void Initialize(Game game)
        {
            gameObject.SetActive(false);
            this.game = game;
            OnInitialize();
        }
        public void Init()
        {
            gameObject.SetActive(true);
            OnInit();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }
        public void Next() { game.Next(); }

        public virtual void OnInitialize() { }
        public virtual void OnInit() { }
        public virtual void OnHide() { }

    }
}
