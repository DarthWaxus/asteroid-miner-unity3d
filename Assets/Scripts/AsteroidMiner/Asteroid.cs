using System;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AsteroidMiner
{
    public class Asteroid : MonoBehaviour
    {
        public List<Uranus> uranuses;
        public Transform view;
        public SoundID collisionSound;
        public StageObject stageObject;

        private void Awake()
        {
            stageObject ??= GetComponent<StageObject>();
        }

        public void Consume(float amount)
        {
            Uranus uranus = uranuses.Find(u => u.amount >= amount);
            if (uranus)
            {
                uranus.Consume(amount);
                view.DOPunchScale(transform.localScale * 0.1f, 0.1f);
            }
        }
        public void Recycle()
        {
            foreach (Uranus uranus in uranuses)
            {
                uranus.SetAmount(Random.Range(0, 5));
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("MainBase"))
            {
                BroAudio.Play(collisionSound, transform.position);
            }
            if (other.collider.CompareTag("MainBase") || other.collider.CompareTag("Asteroid"))
            {
                stageObject.stage.asteroidCollisionFx.transform.position = other.contacts[0].point;
                stageObject.stage.asteroidCollisionFx.Play();
            }
        }
    }
}