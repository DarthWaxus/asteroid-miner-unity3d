using System;
using UnityEngine;

namespace AsteroidMiner
{
    public class Game : MonoBehaviour
    {
        public Stage stage;

        private void Start()
        {
            stage.Init();
        }
    }
}