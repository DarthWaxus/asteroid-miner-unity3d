using System.Collections.Generic;
using UnityEngine;

namespace AsteroidMiner
{
    public class Asteroid : MonoBehaviour
    {
        public List<Uranus> uranuses = new List<Uranus>();

        public void Consume(float amount)
        {
            Uranus uranus = uranuses.Find(u => u.amount > 0);
            if (uranus) uranus.Consume(amount);
        }
    }
}