using System;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;
using Unity.VisualScripting;
using UnityEngine;

namespace AsteroidMiner
{
    public class Player : MonoBehaviour
    {
        public bool onGround = true;
        private Rigidbody2D rb;
        public float jumpPower = 10f;
        public StageObject landedObject;
        public Asteroid landedAsteroid;
        public MainBase mainBase;
        public float uranusAmount = 0;
        public float uranusAmountMax = 50f;
        public SoundID jumpSound;
        public SoundID landSound;
        public SoundID consumeSound;
        public SoundID fullfillSound;
        public SoundID gameOverSound;
        
        public StageObject stageObject;

        private void Awake()
        {
            rb ??= GetComponent<Rigidbody2D>();
            stageObject ??= GetComponent<StageObject>();
        }

        public void Jump()
        {
            if (!onGround) return;
            onGround = false;
            landedObject = null;
            landedAsteroid = null;
            mainBase = null;
            transform.parent = null;
            rb.simulated = true;
            rb.angularVelocity = 0;
            EventManager.ShakeCamera.Invoke(0.2f);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            if(jumpSound.IsValid()) BroAudio.Play(jumpSound, transform.position);
            StopAllCoroutines();
        }

        public void Land(StageObject stageObject)
        {
            if (onGround) return;
            stageObject.stage.asteroidCollisionFx.transform.position = transform.position;
            stageObject.stage.asteroidCollisionFx.Play();
            EventManager.ShakeCamera.Invoke(0.3f);
            onGround = true;
            landedAsteroid = stageObject.GetComponent<Asteroid>();
            mainBase = stageObject.GetComponent<MainBase>();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.simulated = false;
            transform.SetParent(stageObject.transform);
            transform.rotation = stageObject.GetRotationOnCircle(transform.position);
            landedObject = stageObject;
            if(landSound.IsValid()) BroAudio.Play(landSound, transform.position);
            StartCoroutine(ConsumeRoutine());
        }

        IEnumerator ConsumeRoutine()
        {
            while (onGround)
            {
                if (landedAsteroid && uranusAmount < uranusAmountMax)
                {
                    landedAsteroid.Consume(1);
                    uranusAmount += 1;
                    EventManager.ShakeCamera.Invoke(0.01f);
                    if(consumeSound.IsValid()) BroAudio.Play(consumeSound, transform.position);
                    EventManager.PlayerAmountChanged.Invoke(uranusAmount, uranusAmountMax);
                }

                if (mainBase && uranusAmount > 0)
                {
                    uranusAmount -= 1;
                    if(fullfillSound.IsValid()) BroAudio.Play(fullfillSound, transform.position);
                    EventManager.PlayerAmountChanged.Invoke(uranusAmount, uranusAmountMax);
                    mainBase.AddUranus(1);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            StageObject stageObject = other.gameObject.GetComponent<StageObject>();
            if (stageObject && !onGround && (stageObject.type == StageObjectType.Asteroid ||
                                             stageObject.type == StageObjectType.MainBase))
            {
                Land(stageObject);
            }
        }
    }
}