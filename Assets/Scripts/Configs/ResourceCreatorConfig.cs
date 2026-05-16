using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Resource Config", menuName = "Config/ResourceConfig")]
    public class ResourceCreatorConfig : ScriptableObject
    {
        [field: SerializeField] public  float ProduceDuration = 3f;
        [field: SerializeField] public  float WaitingeDuration = 2f;
    }
    
    [CreateAssetMenu(fileName = "Worker Config", menuName = "Config/WorkerConfig")]
    public class WorkerConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed = 2f;
        [field: SerializeField] public float ArrivalThreshold = 0.7f;

        public float ArrivalThresholdSqr => ArrivalThreshold * ArrivalThreshold;
    }
}