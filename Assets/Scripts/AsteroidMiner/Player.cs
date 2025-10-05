using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            rb ??= GetComponent<Rigidbody2D>();
        }

        public void Jump()
        {
            if(!onGround) return;
            onGround = false;
            landedObject = null;
            landedAsteroid = null;
            transform.parent = null;
            rb.simulated = true;
            rb.angularVelocity = 0;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            StopAllCoroutines();
        }

        public void Land(StageObject stageObject)
        {
            if (onGround) return;
            onGround = true;
            landedAsteroid = stageObject.GetComponent<Asteroid>();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.simulated = false;
            transform.SetParent(stageObject.transform);
            transform.rotation = stageObject.GetRotationOnCircle(transform.position);
            landedObject = stageObject;
            StartCoroutine(ConsumeRoutine());
        }

        IEnumerator ConsumeRoutine()
        {
            while (onGround && landedAsteroid)
            {
                landedAsteroid.Consume(1);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            StageObject stageObject = other.gameObject.GetComponent<StageObject>();
            if (stageObject&& !onGround && (stageObject.type == StageObjectType.Asteroid || stageObject.type == StageObjectType.MainBase))
            {
                Land(stageObject);
            }
        }
    }
}