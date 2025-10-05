using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AsteroidMiner
{
    public class Stage : MonoBehaviour
    {
        public int currentAsteroidsNum = 0;
        public int maxAsteroidsNum = 10;
        public float startRadius = 2000;
        public List<GameObject> asteroidPrefabs;
        public Player player;
        public MainBase mainBase;
        public GameObject playerPrefab;
        public GameObject mainBasePrefab;

        private void Awake()
        {
            EventManager.StageObjectDestroyed.AddListener(OnStageObjectDestroyed);
        }

        private void OnStageObjectDestroyed(StageObject obj)
        {
            if(obj.type == StageObjectType.Asteroid)
            {
                currentAsteroidsNum--;
                if (currentAsteroidsNum < maxAsteroidsNum)
                {
                    AddAsteroid(startRadius, true);
                }
            }
            Destroy(obj.gameObject);
        }

        public void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                AddAsteroid(5, false);
            }
            this.mainBase = AddMainBase();
            this.player = AddPlayer();
            
        }

        public Player AddPlayer()
        {
            Player player = Instantiate(playerPrefab, mainBase.transform).GetComponent<Player>();
            player.transform.position = mainBase.stageObject.GetRandomPositionOnCircle();
            player.transform.rotation = mainBase.stageObject.GetRotationOnCircle(player.transform.position);
            return player;
        }
        
        public MainBase AddMainBase()
        {
            return Instantiate(mainBasePrefab, transform).GetComponent<MainBase>();
        }

        public StageObject AddAsteroid(float radius, bool initVelocity = false)
        {
            GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
            StageObject obj = Instantiate(asteroid, transform).GetComponent<StageObject>();
            obj.transform.position = Random.insideUnitCircle.normalized * radius;

            if (initVelocity)
            {
                Vector2 direction = -obj.transform.position;
                direction += Random.insideUnitCircle * 300f;
                obj.rb.linearVelocity = direction.normalized * (Random.Range(50f, 150f) / 100);
            }

            obj.rb.AddTorque(Random.Range(-20, 20));
            currentAsteroidsNum++;
            return obj;
        }
    }
}