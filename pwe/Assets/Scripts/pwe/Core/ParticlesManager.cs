using System.Collections;
using UnityEngine;

namespace Pwe.Core
{
    public class ParticlesManager : MonoBehaviour
    {
        [SerializeField] Transform container;
        public enum types
        {
            pick,
            drop
        }
        [SerializeField] UIParticles pick;
        [SerializeField] UIParticles drop;
        [SerializeField] UIParticles win;

        void Start()
        {
            Events.OnAddParticles += OnAddParticles;
            Events.OnWinParticles += OnWinParticles;
        }
        void OnDestroy()
        {
            Events.OnAddParticles -= OnAddParticles;
            Events.OnWinParticles -= OnWinParticles;
        }
        void OnWinParticles()
        {
            UIParticles particle = Instantiate(win, container);
            particle.transform.localPosition = new Vector2(450, 0);
            StartCoroutine(ParticleOn(particle, 50));
        }
        void OnAddParticles(types type, Vector2 pos, string nameID = "")
        {
            UIParticles p;
            switch (type) {
                case types.pick: p =  pick; break;
                default: p = drop; break;
            }
            UIParticles particle = Instantiate(p, container);
            particle.transform.position = pos;
            if (nameID != "")
                particle.Init(nameID);
            StartCoroutine(ParticleOn(particle, 2));
        }
        IEnumerator ParticleOn(UIParticles go, int duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(go.gameObject);
        }
    }

}