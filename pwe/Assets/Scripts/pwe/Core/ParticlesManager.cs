using System.Collections;
using UnityEngine;

namespace Pwe.Core
{
    public class ParticlesManager : MonoBehaviour
    {
        [SerializeField] Transform container;
        [SerializeField] GameObject particle;
        void Start()
        {
            Events.OnAddParticles += OnAddParticles;
        }
        void OnDestroy()
        {
            Events.OnAddParticles -= OnAddParticles;
        }
        void OnAddParticles(Vector2 pos)
        {
            GameObject go = Instantiate(particle, container);
            go.transform.position = pos;
            StartCoroutine(ParticleOn(go));
        }
        IEnumerator ParticleOn(GameObject go)
        {
            yield return new WaitForSeconds(2);
            Destroy(go.gameObject);
        }
    }

}