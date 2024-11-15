using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pwe.Games.SolarSystem
{
    public class StaticObstacles : MonoBehaviour {
        
        [SerializeField] OrbitalItem[] items;
        // Start is called before the first frame update
        void Start() {
            items = GetComponentsInChildren<OrbitalItem>();
        }
        public void SetObstacles(System.Action<PlanetName> OnPlanetClicked) {
            for (int i = 0; i < items.Length; i++) {
                items[i].Init(i, (correct) => {
                    Debug.Log("# " + PlanetName.none + ": " + correct);
                    OnPlanetClicked(PlanetName.none);
                });
            }
        }
    }
}
