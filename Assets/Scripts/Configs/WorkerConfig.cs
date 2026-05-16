using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Worker Config", menuName = "Config/WorkerConfig")]
    public class WorkerConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed = 2f;
        [field: SerializeField] public float ArrivalThreshold = 0.7f;

        public float ArrivalThresholdSqr => ArrivalThreshold * ArrivalThreshold;
    }
}