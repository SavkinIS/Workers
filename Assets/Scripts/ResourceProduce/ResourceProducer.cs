using System.Collections;
using Configs;
using UnityEngine;

public class ResourceProducer : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private Transform _resourceHolder;
    [SerializeField] private ResourceProducerConfig _resourceConfig;

    private WaitForSeconds _produceTime;
    private WaitForSeconds _waitingTime;
    private Coroutine _produceCoroutine;
    private bool _isActive = true;

    private void Awake()
    {
        _produceTime = new WaitForSeconds(_resourceConfig.ProduceDuration);
        _waitingTime = new WaitForSeconds(_resourceConfig.WaitingeDuration);
    }

    private void Start()
    {
        _produceCoroutine = StartCoroutine(ProduceCoroutine());
    }

    private IEnumerator ProduceCoroutine()
    {
        while (_isActive)
        {
            ResourceItem resourceItem = _resourceSpawner.Spawn();

            resourceItem.AttachTo(_resourceHolder);
            
            yield return _produceTime;
            resourceItem.SetCompleted();
            yield return _waitingTime;
        }
    }
}