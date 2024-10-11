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

        private void Start()
        {
            riveTexture.Init();
        }
        public void Clicked()
        {
            string s = input.text;
            string s2 = input2.text;
            riveTexture.PlayStateMachine(s, s2);
        }
    }
}
