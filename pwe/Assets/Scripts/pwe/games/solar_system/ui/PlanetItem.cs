using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    public class PlanetItem : MonoBehaviour
    {
        [SerializeField] Image image;
        public PlanetData data;

        public void Init(PlanetData data)
        {
            this.data = data;
        }

    }
}