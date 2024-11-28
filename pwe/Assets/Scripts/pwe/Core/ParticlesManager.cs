using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
        [SerializeField] GameObject pick;
        [SerializeField] GameObject drop;
        [SerializeField] GameObject win;
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
            GameObject particle = Instantiate(win, container);
            particle.transform.localPosition = new Vector2(450, 0);
            StartCoroutine(ParticleOn(particle, 50));
        }
        void OnAddParticles(types type, Vector2 pos)
        {
            GameObject p;
            switch (type) {
                case types.pick: p =  pick; break;
                default: p = drop; break;
            }    
            GameObject particle = Instantiate(p, container);
            particle.transform.position = pos;
            StartCoroutine(ParticleOn(particle, 2));
        }
        IEnumerator ParticleOn(GameObject go, int duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(go.gameObject);
        }
    }

}