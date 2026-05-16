using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ResourceProduceService _resourceProduceService;
    [SerializeField] private Storage _storage;

    private void OnEnable()
    {
        _resourceProduceService.ProduceCompleted += _storage.ResourceCreated;
    }
    
    private void OnDisable()
    {
        _resourceProduceService.ProduceCompleted -= _storage.ResourceCreated;
    }
}
