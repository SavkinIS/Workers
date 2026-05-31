using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Worker Config", menuName = "Config/WorkerConfig")]
    public class WorkerConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _arrivalThreshold = 0.7f;
        [SerializeField] private float _arrivalBaseThreshold = 1.5f;

        public float ArrivalThreshold => _arrivalThreshold ;
        public float ArrivalBaseThreshold => _arrivalBaseThreshold;
        public float MoveSpeed => _moveSpeed;
    }
}