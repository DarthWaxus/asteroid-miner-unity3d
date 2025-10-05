using System;
using UnityEngine;

namespace AsteroidMiner
{
    public class MainBase : MonoBehaviour
    {
        public StageObject stageObject;

        private void Awake()
        {
            stageObject ??= GetComponent<StageObject>();
        }
    }
}