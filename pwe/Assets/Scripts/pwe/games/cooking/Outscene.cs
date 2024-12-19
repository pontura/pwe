using Pwe.Games.Cooking;
using Rive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Audio;

namespace Pwe.Games.Cooking.UI
{
    public class Outscene : Cutscene
    {
        [SerializeField] SlicerCreator slicer;

        public override void OnInit()
        {
            base.OnInit();
            slicer.Init();

            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("outro_cutscene_music").clip, AudioManager.channels.UI);
            Invoke("AudioFXDelayed", 0.1f);
        }
        void AudioFXDelayed()
        {
            YaguarLib.Events.Events.PlayGenericSound(Game.Sounds.GetClip("outro_cutscene_fx").clip, AudioManager.channels.GAME);
        }
        public override void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            
        }
    }

}