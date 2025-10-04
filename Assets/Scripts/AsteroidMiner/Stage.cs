using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AsteroidMiner
{
    public class Stage : MonoBehaviour
    {
        public float startRadius = 2000;
        public List<GameObject> asteroidPrefabs;

        private void Start()
        {
            
        }

        public void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                AddAsteroid(5);
            }
        }
        public void AddAsteroid(float radius)
        {
            GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
            StageObject obj = Instantiate(asteroid, transform).GetComponent<StageObject>();
            obj.transform.position = Random.insideUnitCircle.normalized * radius;
            
        }
    }
}