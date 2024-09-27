using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaguarLib.UI
{
    [RequireComponent(typeof(Animator))]
    public class ButtonToggle : ButtonUIText
    {
        Animator anim;
        [SerializeField] AnimationClip anim_on;
        [SerializeField] AnimationClip anim_off;
        System.Action<bool> ChangeState;
        bool isOn;
        public void OnInitToggle(System.Action<bool> ChangeState, bool isOn = true)
        {
            anim = GetComponent<Animator>();
            this.ChangeState = ChangeState;
            Init(OnClicked); 
        }
        public void SetState(string text, bool isOn = true)
        {            
            SetText(text);            
            this.isOn = isOn;
            SetAnim();
        }
        void OnClicked()
        {
            isOn = !isOn;
            ChangeState(isOn);
            SetAnim();
        }
        void SetAnim()
        {
            if(anim == null) return;

            if(isOn)
                anim.Play(anim_on.name);
            else
                anim.Play(anim_off.name);
        }
    }

}