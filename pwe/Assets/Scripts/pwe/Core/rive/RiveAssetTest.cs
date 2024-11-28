using Pwe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Core
{
    public class RiveAssetTest : MonoBehaviour
    {
        [SerializeField] string riveName;
        RiveRawImage riveTexture;

        void Start()
        {
            riveTexture = GetComponent<RiveRawImage>();
            riveTexture.Init(riveName + ".riv", OnReady);
        }
        void OnReady() { }
    }
}
