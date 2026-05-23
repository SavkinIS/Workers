using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;
    
    private List<ResourceItem> _collectedResources = new List<ResourceItem>();
    
    public event Action<int> ChangedResourceAmount;

    private void OnEnable()
    {
        _scanner.Scanned += SendWorker;
        
        foreach (var worker in _workers)
        {
            worker.ResourcePutted += Claim;
        }
    }

    private void Start()
    {
        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget);
        }
        
        ChangedResourceAmount?.Invoke(_collectedResources.Count);
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourcePutted -= Claim;
        }
        
        _scanner.Scanned -= SendWorker;
    }

    public void Claim(ResourceItem resourceItem)
    {
        resourceItem.Disable(_putTarget);
        _collectedResources.Add(resourceItem);
        ChangedResourceAmount?.Invoke(_collectedResources.Count);
    }

    private void SendWorker(List<ResourceItem>  scannedResources)
    {
        foreach (var resourceItem in scannedResources)
        {
            Worker worker =  GetFreeWorker();

            if (worker != null)
            {
                worker.SendToResource(resourceItem);
            }
        }
    }

    private Worker GetFreeWorker()
    {
        foreach (var worker in _workers)
        {
            if (worker.IsBusy == false)
                return worker;
        }
        
        return null;
    }
}