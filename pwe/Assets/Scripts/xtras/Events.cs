using Meme.UI;
using UnityEngine;

namespace Meme
{
    public static class Events
    {

        public static System.Action<int, Vector2, float, float> OnFlyingParticles = delegate { };
        public static System.Action<float, float, float> OnFlyingPArrives = delegate { };

        public static System.Action InitApp = delegate { };
        //UI 
        public static System.Action<System.Action> OnBoarding = delegate { };
        public static System.Action<bool> LoadingMain = delegate { };
        public static System.Action<bool> Loading = delegate { };
        public static System.Action UserDataUpdated = delegate { };
        public static System.Action<bool> Mute = delegate { };
        public static System.Action<AudioManager.types> OnPlaySound = delegate { };
        public static System.Action<AudioManager.types, string> OnPlaySoundInChannel = delegate { };
        public static System.Action<string, System.Action> OnPopup = delegate { };
        public static System.Action OpenSettings = delegate { };
        public static System.Action UserDataRefreshed = delegate { };
        public static System.Action InviteFriends = delegate { };
        public static System.Action OpenBuyEnergy = delegate { };
        public static System.Action OnEnergyLevelUp = delegate { };
        //GAME
        public static System.Action<Vector2, int> OnGameClick = delegate { };
        public static System.Action<int, int> UpdateEnergy = delegate { };
        public static System.Action<int> UpdatePoints = delegate { };
        public static System.Action LevelComplete = delegate { };
        public static System.Action<bool> OutOfEnergy = delegate { };


    }
}