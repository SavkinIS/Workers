using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Resource Config", menuName = "Config/ResourceConfig")]
    public class ResourceProducerConfig : ScriptableObject
    {
        [SerializeField] private float _produceDuration = 4f;
        [SerializeField] private float waitingDuration = 2f;

        public float ProduceDuration => _produceDuration;
        public float WaitingDuration => waitingDuration;
    }
}