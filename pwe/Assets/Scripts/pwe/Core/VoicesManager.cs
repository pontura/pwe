using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Pwe.Core
{
    public class VoicesManager : MonoBehaviour
    {
        [SerializeField] AudioClip[] numbers;
        [SerializeField] AudioSource audioSource;

        private void Start()
        {
            Events.OnSayNumber += OnSayNumber;
        }
        private void OnDestroy()
        {
            Events.OnSayNumber -= OnSayNumber;
        }
        private void OnSayNumber(int num)
        {
            AudioClip ac = numbers[num - 1];
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
}
