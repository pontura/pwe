using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pwe.Games.SolarSystem
{
    public class PlanetsManager : MonoBehaviour
    {
        [SerializeField] SpaceData spaceData;
        [SerializeField] Transform sun;
        [SerializeField] Transform planetsContainer;
        [SerializeField] Transform orbitsContainer;
        [SerializeField] Planet planet_prefab;

        public void Init(PlanetsData planetsData) {
            spaceData._minmaxDistance = planetsData.GetMinMaxDistance();
            spaceData._minmaxSize = planetsData.GetMinMaxSize();
            spaceData._minmaxSpeed = planetsData.GetMinMaxSpeed();

            RemoveAllOrbits();
            foreach (SpaceData.LevelItem li in spaceData.LevelItems) {
                Debug.Log("#" + li.planetName.ToString());
                if ((int)li.planetName > 0) {
                    PlanetData pd = planetsData.planets.Find(x => x.planetName == li.planetName);
                    Debug.Log("#" + (pd!=null));
                    AddPlanet(pd, li.orbitalPath);
                }

                OrbitalPath op = Instantiate(li.orbitalPath, orbitsContainer);                
            }
        }

        public void AddPlanet(PlanetData pd) {
            Planet p = Instantiate(planet_prefab, planetsContainer);            
            p.Init(sun, spaceData, pd);
        }

        public void AddPlanet(PlanetData pd, OrbitalPath path) {
            Planet p = Instantiate(planet_prefab, planetsContainer);
            p.Init(spaceData, pd, path);
        }

        public void RemoveAllPlanets() {
            foreach (Transform child in planetsContainer) {
                if(child!=sun)
                    Destroy(child.gameObject);
            }
        }

        public void RemoveAllOrbits() {
            foreach (Transform child in orbitsContainer) {
                if (child != sun)
                    Destroy(child.gameObject);
            }
        }

        public void ToggleOrbits() {
            foreach(OrbitalPath op in orbitsContainer.GetComponentsInChildren<OrbitalPath>()) {
                op.ToggleLine();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlanetsManager))]
    class PlanetsManagerEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            PlanetsManager planetsManager = (PlanetsManager)target;            
            if (GUILayout.Button("Toggle Orbits")) {
                planetsManager.ToggleOrbits();
            }
        }
    }
#endif
}