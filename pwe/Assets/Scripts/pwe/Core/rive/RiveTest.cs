using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Pwe
{
    public class RiveTest : MonoBehaviour
    {
        [SerializeField] InputField input;
        [SerializeField] InputField input2;

        public RiveTexture rive;
        [SerializeField] protected Rive.Asset asset;

        private void Start()
        {
            // riveTexture.Init(riveName, null);
            rive.Init(asset);
            List<string> arr = new List<string>();
            arr.Add("Artboard");
            rive.AddArtBoards(arr);
            rive.ActivateArtboard("Artboard");
        }
        public void Clicked()
        {
            string s = input.text;
            string s2 = input2.text;
            // riveTexture.PlayStateMachine(s, s2);
            rive.SetTriggerInArtboard(s, s2);
        }
        private void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");

            //// Access specific event properties
            //if (reportedEvent.Name.StartsWith("click"))
            //{
            //    var click = reportedEvent["click"];
            //    var type = reportedEvent["type"];
            //    Debug.Log($"click: {click}");
            //    Debug.Log($"type: {type}");
            //}
        }
    }
}
