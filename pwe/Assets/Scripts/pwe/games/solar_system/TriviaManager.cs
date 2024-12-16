using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.SolarSystem
{
    public class TriviaManager : MonoBehaviour
    {
        RiveTexture _riveTexture;
        List<PlanetName> _itemsNames;

        System.Action<PlanetName> _onSelected;

        public void Init(RiveTexture rt, List<PlanetName> itemsNames, System.Action<PlanetName> onSelected) {
            _riveTexture = rt;
            _itemsNames = itemsNames;
            _onSelected = onSelected;
            _riveTexture.SetNumberInArtboard("Trivia", "trivia_btn_count", itemsNames.Count);
            for (int i = 0; i < itemsNames.Count; i++) {
                _riveTexture.SetNumberInArtboard("Trivia/Trivia_Btn_"+(i+1), "planet_id", (int)itemsNames[i]);
            }
        }

        public void ShowTrivia(bool enable) {
            _riveTexture.SetBool("game", "show_trivia", enable);            
        }

        public void OnButtonPressed(PlanetName selected, int buttonId) {
            if (buttonId-1 >= 0 && buttonId-1 < _itemsNames.Count) {
                _onSelected(_itemsNames[buttonId-1]);
                SetButtonState(buttonId, selected == _itemsNames[buttonId-1]);
            } else
                Debug.LogError("Error trivia button index out of range: " + (buttonId-1));
        }

        void SetButtonState(int btnId, bool isRight) {
            int val = isRight ? 2 : 1;
            _riveTexture.SetNumberInArtboard("Trivia/Trivia_Btn_" + btnId, "trivia_btn_state", val);
            StartCoroutine(ResetTriviaBtns(btnId));
        }

        System.Collections.IEnumerator ResetTriviaBtns(int btnId) {
            yield return new WaitForSecondsRealtime(0.3f);
            _riveTexture.SetNumberInArtboard("Trivia/Trivia_Btn_" + btnId, "trivia_btn_state", 0);
            //Game.rive.SetNumber("game", "trivia_state_btn_" + (int)pressed, 0);
        }

    }
}
