using System;
using System.Collections;
using Configs;
using UnityEngine;

public class ResourceCreator : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private Transform _resourceHolder;
    [SerializeField] private ResourceCreatorConfig _resourceConfig;

    private WaitForSeconds _produceTime;
    private WaitForSeconds _waitingTime;
    private Coroutine _produceCoroutine;

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
        while (true)
        {
            ResourceItem resourceItem = _resourceSpawner.Spawn();
            resourceItem.SetParent(_resourceHolder);
            resourceItem.ResetPosition();

            yield return _produceTime;
            resourceItem.SetCompleted();
            yield return _waitingTime;
        }
    }
}