using UnityEngine.Events;

namespace AsteroidMiner
{
    public class EventManager
    {
        public static UnityEvent<StageObject> StageObjectDestroyed = new UnityEvent<StageObject>();
    }
}