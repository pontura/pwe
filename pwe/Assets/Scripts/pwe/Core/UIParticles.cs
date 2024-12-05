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

        public void Init(string nameID)
        {
            if (byId.Length > 0)
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
