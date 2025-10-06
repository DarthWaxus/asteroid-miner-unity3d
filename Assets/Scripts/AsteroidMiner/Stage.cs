using System;
using System.Collections.Generic;
using Ami.BroAudio;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AsteroidMiner
{
    public class Stage : MonoBehaviour
    {
        public int currentAsteroidsNum = 0;
        public int maxAsteroidsNum = 30;
        public float startRadius = 2000;
        public List<GameObject> asteroidPrefabs;
        List<Asteroid> asteroids = new List<Asteroid>();
        public Player player;
        public MainBase mainBase;
        public GameObject playerPrefab;
        public GameObject mainBasePrefab;
        public Game game;
        public ParticleSystem asteroidCollisionFx;

        private void Awake()
        {
            EventManager.StageObjectDestroyed.AddListener(OnStageObjectDestroyed);
        }

        private void OnStageObjectDestroyed(StageObject obj)
        {
            if (obj.type == StageObjectType.Asteroid)
            {
                currentAsteroidsNum--;
                if (currentAsteroidsNum < maxAsteroidsNum)
                {
                    int notActiveAmount = asteroids.FindAll(a => !a.gameObject.activeSelf).Count;
                    int diff = maxAsteroidsNum - currentAsteroidsNum;
                    int max = Mathf.Clamp(Random.Range(1, diff + 1), 0, notActiveAmount);
                    for (int i = 0; i < max; i++)
                    {
                        Asteroid asteroid = asteroids.Find(a => !a.gameObject.activeSelf);
                        asteroid.gameObject.SetActive(true);
                        AddAsteroid(asteroid, startRadius, true);
                    }
                }
            }

            if (obj.type == StageObjectType.Player)
            {
                BroAudio.Play(player.gameOverSound);
                game.GameOver();
            }

            obj.gameObject.SetActive(false);
        }

        public void Init()
        {
            for (int i = 0; i < maxAsteroidsNum; i++)
            {
                Asteroid asteroid = CreateAsteroid();
                AddAsteroid(asteroid, Random.Range(5, startRadius), true);
            }

            mainBase = AddMainBase();
            player = AddPlayer();
        }

        public Player AddPlayer()
        {
            Player player = Instantiate(playerPrefab, mainBase.transform).GetComponent<Player>();
            player.transform.position = mainBase.stageObject.GetRandomPositionOnCircle();
            player.transform.rotation = mainBase.stageObject.GetRotationOnCircle(player.transform.position);
            player.landedObject = mainBase.stageObject;
            player.stageObject = player.GetComponent<StageObject>();
            player.stageObject.stage = this;
            return player;
        }

        public MainBase AddMainBase()
        {
            MainBase mainBase = Instantiate(mainBasePrefab, transform).GetComponent<MainBase>();
            mainBase.stageObject = mainBase.GetComponent<StageObject>();
            mainBase.stageObject.stage = this;
            return mainBase;
        }

        public Asteroid CreateAsteroid()
        {
            Asteroid asteroid= Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], transform)
                .GetComponent<Asteroid>();
            asteroid.stageObject = asteroid.GetComponent<StageObject>();
            asteroid.stageObject.stage = this;
            asteroids.Add(asteroid);
            return asteroid;
        }

        public Asteroid AddAsteroid(Asteroid asteroid, float radius, bool initVelocity = false)
        {
            StageObject obj = asteroid.GetComponent<StageObject>();
            asteroid.transform.position = Random.insideUnitCircle.normalized * radius;
            asteroid.Recycle();
            asteroid.transform.localScale = Vector3.one*Random.Range(0.8f, 1.1f);
            if (initVelocity)
            {
                Vector2 direction = -obj.transform.position;
                direction += Random.insideUnitCircle * 300f;
                obj.rb.linearVelocity = direction.normalized * (Random.Range(150f, 300f) / 100);
            }

            currentAsteroidsNum++;
            return asteroid;
        }
    }
}