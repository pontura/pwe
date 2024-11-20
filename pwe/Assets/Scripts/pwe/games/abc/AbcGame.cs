using Pwe.Games.SolarSystem.UI;
using Pwe.Games.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YaguarLib.Xtras;
using YaguarLib.UI;
using System.Linq;
using YaguarLib.Audio;

namespace Pwe.Games.Abc
{
    public class AbcGame : GameMain
    {        

        public override void OnInitialize() {
            
        }

        public override void OnInit() {
            base.OnInit();

            
        }

        private void OnDestroy() {
            
        }
                
        
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AbcGame))]
    class AbcGameEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            AbcGame solarSystem = (AbcGame)target;
            //SolarSystem.SetPlanets = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
            
            /*if (GUILayout.Button("Reload Planets")) {
                solarSystem.InitPlanets();
            }*/
        }
    }
#endif
}
