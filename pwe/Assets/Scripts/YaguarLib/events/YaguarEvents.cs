using YaguarLib.Audio;

namespace YaguarLib.Events
{
    public static class Events
    {

        //Audio
        public static System.Action<bool> Mute = delegate { };
        public static System.Action StopAllSounds = delegate { };
        public static System.Action<AudioManager.channels> StopChannel = delegate { };
        public static System.Action OnPlaySounds = delegate { };
        public static System.Action<AudioManager.types> OnPlaySound = delegate { };
        public static System.Action<AudioManager.types, AudioManager.channels> OnPlaySoundInChannel = delegate { };
    }
}