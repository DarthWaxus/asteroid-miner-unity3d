using UnityEngine;

namespace AsteroidMiner
{
    public class StageObject : MonoBehaviour
    {
        public float radius = 0.5f;
        public Rigidbody2D rb;
        public bool dynamic;
        public float rotateSpeed = 0;
        public StageObjectType type = StageObjectType.None;
        public float destroyDistance = 50;
        public Stage stage;

        private void FixedUpdate()
        {
            if (rotateSpeed != 0)
            {
                rb.MoveRotation(rb.rotation + rotateSpeed * Time.fixedDeltaTime);
            }

            if (dynamic)
            {
                if (Vector2.Distance(Vector3.zero, rb.position) > destroyDistance)
                {
                    EventManager.StageObjectDestroyed.Invoke(this);
                }
            }
        }
        public Vector2 GetRandomPositionOnCircle()
        {
            return (Vector2)transform.position + Random.insideUnitCircle.normalized * radius;
        }

        public Quaternion GetRotationOnCircle(Vector2 position)
        {
            Vector2 direction = (position - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle - 90);
        }
    }

    public enum StageObjectType
    {
        None,
        Asteroid,
        MainBase,
        Player,
        Uranus,
    }
}