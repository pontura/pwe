using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pwe.Games.SolarSystem
{
    public class PlanetsManager : MonoBehaviour
    {
        [SerializeField] Transform sun;
        [SerializeField] Transform planetsContainer;
        [SerializeField] Transform orbitsContainer;
        [SerializeField] Planet planet_prefab;

        public event System.Action<PlanetName> OnPlanetClicked;

        public void Init(PlanetsData planetsData, SpaceData spaceData) {
            spaceData._minmaxDistance = planetsData.GetMinMaxDistance();
            spaceData._minmaxSize = planetsData.GetMinMaxSize();
            spaceData._minmaxSpeed = planetsData.GetMinMaxSpeed();

            RemoveAllOrbits();
            int index = 0;
            foreach (SpaceData.LevelItem li in spaceData.LevelItems) {
                Debug.Log("#" + li.planetName.ToString());
                if ((int)li.planetName > 0) {
                    PlanetData pd = planetsData.planets.Find(x => x.planetName == li.planetName);
                    Debug.Log("#" + (pd!=null));
                    AddPlanet(pd, li.orbitalPath, li.colliderRadius, li.animClip, spaceData);
                } else {
                    AddOvni(index, li.orbitalItem, li.orbitalPath, spaceData);
                }

                OrbitalPath op = Instantiate(li.orbitalPath, orbitsContainer);
                index++;
            }
        }

        public void AddPlanet(PlanetData pd, SpaceData spaceData) {
            Planet p = Instantiate(planet_prefab, planetsContainer);            
            p.Init(sun, spaceData, pd);
        }        

        public void AddPlanet(PlanetData pd, OrbitalPath path, float colliderRadius, AnimationClip animClip, SpaceData spaceData) {
            Planet p = Instantiate(planet_prefab, planetsContainer);
            p.Init(spaceData, pd, colliderRadius, path, animClip, (correct) => {
                Debug.Log("# " + pd.planetName + ": " + correct);
                OnPlanetClicked(correct ? pd.planetName : PlanetName.none);
            });
        }

        public void AddOvni(int index, OrbitalItem oi, OrbitalPath path, SpaceData spaceData) {
            OrbitalItem p = Instantiate(oi, planetsContainer);
            p.transform.localPosition = Vector3.zero;
            p.Init(index, spaceData, path, (correct) => {
                Debug.Log("# " + PlanetName.none + ": " + correct);
                OnPlanetClicked(PlanetName.none);
            });
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

        public void Play(bool enable) {
            foreach (OrbitalItem op in planetsContainer.GetComponentsInChildren<OrbitalItem>()) {
                op.Moving = enable;
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