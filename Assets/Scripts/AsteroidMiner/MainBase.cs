using System;
using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;

namespace AsteroidMiner
{
    public class MainBase : MonoBehaviour
    {
        public StageObject stageObject;
        public float uranusAmount = 0;
        public float uranusAmountMax = 100;
        public Transform rocket;
        public GameObject rocketTrail;
        public SoundID winSound;

        private void Awake()
        {
            stageObject ??= GetComponent<StageObject>();
        }

        public void AddUranus(float amount)
        {
            uranusAmount += amount;
            uranusAmount = Math.Clamp(uranusAmount, 0, uranusAmountMax);
            EventManager.RocketAmountChanged.Invoke(uranusAmount, uranusAmountMax);
            if ((int)uranusAmount == (int)uranusAmountMax) EventManager.MissionCompleted.Invoke();
        }

        public void StartRocket()
        {
            if (winSound.IsValid()) BroAudio.Play(winSound);
            rocket.transform.SetParent(null);
            rocketTrail.SetActive(true);
            rocket.DOMove(rocket.up * 20, 1.5f).SetEase(Ease.InSine);
        }
    }
}