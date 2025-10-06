using UnityEngine.Events;

namespace AsteroidMiner
{
    public class EventManager
    {
        public static UnityEvent<StageObject> StageObjectDestroyed = new UnityEvent<StageObject>();
        public static UnityEvent MissionCompleted = new UnityEvent();
        public static UnityEvent<float,float> PlayerAmountChanged = new UnityEvent<float,float>();
        public static UnityEvent<float,float> RocketAmountChanged = new UnityEvent<float,float>();
        public static UnityEvent<float> ShakeCamera = new UnityEvent<float>();
    }
}