using Pwe.Core;
using Pwe.Games.Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pwe.Games
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] RiveRawImage riveRawImage;
        System.Action OnReady;
        private void Start()
        {
            riveRawImage.gameObject.SetActive(false);
            Events.OnTransition += OnTransition;
        }
        private void OnDestroy()
        {
            Events.OnTransition -= OnTransition;
        }
        void OnTransition(System.Action OnReady)
        {
            riveRawImage.gameObject.SetActive(true);
            print("OnTransition");
            this.OnReady = OnReady;
            riveRawImage.Init("Cooking/cutscenes/transitioncooking.riv", OnLoaded);
        }
        void OnLoaded()
        {
            Invoke("OnDelayed", 0.5f);        
        }
        void OnDelayed()
        {
            print("OnTransition Done");
            OnReady();
            riveRawImage.SetTrigger("transition");
        }
    }
}