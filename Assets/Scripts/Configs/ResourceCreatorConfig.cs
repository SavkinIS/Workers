using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Resource Config", menuName = "Config/ResourceConfig")]
    public class ResourceCreatorConfig : ScriptableObject
    {
        [field: SerializeField] public  float ProduceDuration = 3f;
        [field: SerializeField] public  float WaitingeDuration = 2f;
    }
}