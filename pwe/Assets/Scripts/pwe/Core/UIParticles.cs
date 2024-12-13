using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Pwe.Core
{
    public class UIParticles : MonoBehaviour
    {
        [Serializable] class ParticlesData
        {
            public string nameID;
            public GameObject go;
        }
        [SerializeField] ParticlesData[] byId;

        public ParticleSystemData[] particlesToColorize;

        [Serializable]
        public class ParticleSystemData
        {
            public ParticleSystem[] particles;
        }

        public void Init(string nameID = "", List<Color> colors = null)
        {
            if (nameID != "" && byId.Length > 0)
                SetParticlesByName(nameID);
            if (colors != null)
                SetParticlesColor(colors);
        }
        void SetParticlesByName(string nameID)
        {
            foreach (ParticlesData p in byId)
                p.go.SetActive(false);

            ParticlesData pData = GetParticleByID(nameID);
            if (pData != null)
                pData.go.SetActive(true);
            else
            {
                Debug.LogError(gameObject.name + " Has No particle ID for " + nameID);
            }
        }
        void SetParticlesColor(List<Color> colors)
        {
            
            foreach (ParticleSystemData pData in particlesToColorize)
            {
                int colorID = 0;
                foreach (ParticleSystem p in pData.particles)
                {
                    if (colorID < colors.Count)
                    {
                        p.startColor = colors[colorID];
                        colorID++;
                    }
                }
            }

        }
        ParticlesData GetParticleByID(string nameID)
        {
            foreach(ParticlesData p in byId)
            {
                if(p.nameID == nameID) return p; 
            }
            return null;
        }
    }
}
