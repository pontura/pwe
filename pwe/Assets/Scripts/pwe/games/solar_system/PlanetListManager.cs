using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.SolarSystem
{
    public class PlanetListManager : MonoBehaviour
    {
        RiveTexture _riveTexture;
        List<PlanetName> _itemsNames;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(RiveTexture rt, List<PlanetName> itemsNames) {
            _riveTexture = rt;
            _itemsNames = itemsNames;

            foreach (PlanetName pn in System.Enum.GetValues(typeof(PlanetName))) {
                _riveTexture.SetBoolInArtboard("planetsList/" + pn.ToString(), "inactive", !_itemsNames.Contains(pn));
            }
        }

        public void SetPlanetDone(PlanetName pn) {
            _riveTexture.SetBoolInArtboard("planetsList/" + pn.ToString(), "face", true);
            _riveTexture.SetTriggerInArtboard("planetsList/" + pn.ToString(), "popup");
        }

        public void Show(bool enable) {
            _riveTexture.SetBoolInArtboard("planetsList", "hide", !enable);
        }
    }
}