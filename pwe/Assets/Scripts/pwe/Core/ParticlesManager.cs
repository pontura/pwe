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
        [SerializeField] GameObject pick;
        [SerializeField] GameObject drop;
        void Start()
        {
            Events.OnAddParticles += OnAddParticles;
        }
        void OnDestroy()
        {
            Events.OnAddParticles -= OnAddParticles;
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
            StartCoroutine(ParticleOn(particle));
        }
        IEnumerator ParticleOn(GameObject go)
        {
            yield return new WaitForSeconds(2);
            Destroy(go.gameObject);
        }
    }

}