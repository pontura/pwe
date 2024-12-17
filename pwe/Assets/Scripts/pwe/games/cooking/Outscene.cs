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
        }
        public override void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            foreach (string channel in reportedEvent.Properties.Keys)
            {
                print("Play cutscene audio  key " + channel + " value: " + reportedEvent.Properties[channel]);
                string audioName = reportedEvent.Properties[channel].ToString();
                AudioClip audioClip = Game.Sounds.GetClip(audioName).clip;
                if (audioClip != null)
                {
                    if (channel == "audio")
                        YaguarLib.Events.Events.PlayGenericSound(audioClip, AudioManager.channels.GAME);
                    else
                        YaguarLib.Events.Events.PlayGenericSound(audioClip, AudioManager.channels.MUSIC);
                }
                else
                {
                    Debug.LogError("No generic audio named: " + audioName);
                }
            }
        }
    }

}