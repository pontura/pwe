using UnityEngine;
using UnityEngine.UI;
using static Pwe.Games.SolarSystem.PlanetsData;

namespace Pwe.Games.SolarSystem.UI
{
    public class PlanetItem : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] GameObject done;
        [field:SerializeField] public PlanetName Planet_Name { get; private set; }        

        public void Init(PlanetData data)
        {
            Planet_Name = data.planetName;
            image.sprite = data.sprite;
        }

        public void SetDone() {
            done.SetActive(true);
        }

    }
}