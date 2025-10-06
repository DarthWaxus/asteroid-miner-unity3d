using System;
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
            rocket.transform.SetParent(null);
            rocket.DOMove(rocket.up * 20, 3).SetEase(Ease.InSine);
        }
    }
}