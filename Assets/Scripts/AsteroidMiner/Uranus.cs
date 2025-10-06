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
            SetAmount(this.amount);
        }

        public void SetAmount(float amount)
        {
            this.amount = amount;
            float v = this.amount / 20f;
            view.localScale = new Vector3(v, v, v);
        }
    }
}