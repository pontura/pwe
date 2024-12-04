using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Games
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] string transitionArtboardName = "transition";
        Game game;
        RiveScreen riveScreen;
        private void Start()
        {
            riveScreen = Camera.main.GetComponent<RiveScreen>();
            game = GetComponent<Game>();
            Events.OnTransition += OnTransition;
        }
        private void OnDestroy()
        {
            Events.OnTransition -= OnTransition;
        }
        void OnTransition(System.Action OnReady, string nextArtboard)
        {
            riveScreen.enabled = true;
           // game.rive.ActivateArtboard(transitionArtboardName);
            print("OnTransition");
            StartCoroutine(Anim(OnReady, nextArtboard));
        }
        IEnumerator Anim(System.Action OnReady, string nextArtboard)
        {
            riveScreen.SetTrigger("init");
            yield return new WaitForSeconds(0.5f);
            riveScreen.SetTrigger("transition");
            game.rive.ActivateArtboard(nextArtboard);
            print("OnTransition Done to: " + nextArtboard);
            OnReady();
            OnReady = null;
            // game.rive.SetTrigger("transition", "transition");
            yield return new WaitForSeconds(0.5f);
            //if(nextArtboard != "")
            //    game.rive.ActivateArtboard(nextArtboard);
            nextArtboard = "";
            riveScreen.enabled = false;
        }
    }
}