using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Worker Config", menuName = "Config/WorkerConfig")]
    public class WorkerConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _arrivalThreshold = 0.7f;

        public float ArrivalThresholdSqr => _arrivalThreshold * _arrivalThreshold;
        public float MoveSpeed => _moveSpeed;
        
    }
}