using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AsteroidMiner
{
    public class Asteroid : MonoBehaviour
    {
        public List<Uranus> uranuses;
        public Transform view;

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
    }
}