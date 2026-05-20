using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Resource Config", menuName = "Config/ResourceConfig")]
    public class ResourceCreatorConfig : ScriptableObject
    {
        [SerializeField] private  float _produceDuration = 4f;
        [SerializeField] private  float _waitingeDuration = 2f;
        
        public float ProduceDuration => _produceDuration;
        public float WaitingeDuration => _waitingeDuration;
    }
}