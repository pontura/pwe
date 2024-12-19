using Pwe.Core;
using Pwe.Games.Cooking;
using Rive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Audio;

namespace Pwe.Games.Cooking.UI
{
    public class Outscene : GameMain
    {
        [SerializeField] SlicerCreator slicer;

        public override void OnInit()
        {
            base.OnInit();
            slicer.Init();

            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("outro_cutscene_music").clip, AudioManager.channels.UI);
            Invoke("AudioFXDelayed", 0.1f);

            switch( (Game as CookingGame).CookingData.Part)
            {
                case "pizza":
                    GetRiveTexture().SetNumber("outro", "porcion", 1); 
                    break;
                case "cake":
                    GetRiveTexture().SetNumber("outro", "porcion", 3);
                    break;
                case "waffle":
                    GetRiveTexture().SetNumber("outro", "porcion", 2);
                    break;
            }
            Invoke("GotoMainMenu", 11);
        }
        void AudioFXDelayed()
        {
            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("outro_cutscene_fx").clip, AudioManager.channels.GAME);
        }
        void GotoMainMenu()
        {
            Events.OnTransition(OnTransitionDone, "");
        }
        void OnTransitionDone()
        {
            Next();
        }
    }

}