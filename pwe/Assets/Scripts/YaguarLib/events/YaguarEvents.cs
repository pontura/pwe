﻿using YaguarLib.Audio;

namespace YaguarLib.Events
{
    public static class Events
    {

        //Audio
        public static System.Action<bool> Mute = delegate { };
        public static System.Action<AudioManager.types> OnPlaySound = delegate { };
        public static System.Action<AudioManager.types, string> OnPlaySoundInChannel = delegate { };
    }
}