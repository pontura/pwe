using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Games
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] Canvas canvas;
        [SerializeField] Game game;

        private void Awake()
        {
            game = GetComponent<Game>();
            canvas.gameObject.SetActive(false);
            Events.OnTransition += OnTransition;
        }
        private void OnDestroy()
        {
            Events.OnTransition -= OnTransition;
        }
        void OnTransition(System.Action OnReady, string nextArtboard)
        {
            print("OnTransition");
            StartCoroutine(Anim(OnReady, nextArtboard));
        }
        IEnumerator Anim(System.Action OnReady, string nextArtboard)
        {
            anim.Play("transition_in");
            canvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            anim.Play("transition_out");
            game.rive.ActivateArtboard(nextArtboard);
            OnReady();
            OnReady = null;
            yield return new WaitForSeconds(0.7f);
            if(nextArtboard != "")
                game.rive.ActivateArtboard(nextArtboard);
            canvas.gameObject.SetActive(false);
        }
    }
}