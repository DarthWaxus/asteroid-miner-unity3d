using UnityEngine;

namespace AsteroidMiner
{
    public class Uranus : MonoBehaviour
    {
        public float amount = 100;
        public Transform view;

        public void Consume(float amount)
        {
            if (this.amount <= 0) return;
            this.amount -= amount;
            if (this.view)
            {
                this.view.localScale = Vector3.one * (this.amount / 100f);
            }
        }
    }
}