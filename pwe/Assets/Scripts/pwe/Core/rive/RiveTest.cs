using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Pwe
{
    public class RiveTest : MonoBehaviour
    {
        [SerializeField] InputField input;
        [SerializeField] InputField input2;
        [SerializeField] RiveTexture riveTexture;
        [SerializeField] string riveName;

        private void Start()
        {
            riveTexture.Init(riveName, null);
            riveTexture.OnRiveEvent += RiveScreen_OnRiveEvent;
        }
        public void Clicked()
        {
            string s = input.text;
            string s2 = input2.text;
           // riveTexture.PlayStateMachine(s, s2);
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
