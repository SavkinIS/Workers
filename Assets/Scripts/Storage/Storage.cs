using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;

    private ResourceData _resourceData = new ResourceData();
    private WorkersData _workersData;

    public event Action<int> ChangedResourceAmount;

    private void OnEnable()
    {
        _scanner.Scanned += AddResources;

        foreach (var worker in _workers)
        {
            worker.ResourcePutted += Claim;
            worker.WorkCompleted += WorkerTaskCompleted;
        }
    }

    private void WorkerTaskCompleted(Worker worker)
    {
        _workersData.AddFreeWorker(worker);
        
        SendWorker();
    }

    private void Start()
    {
        _workersData = new WorkersData(_workers);
        
        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget);
        }

        ChangedResourceAmount?.Invoke(_resourceData.CollectedResources);
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourcePutted -= Claim;
            worker.WorkCompleted -= WorkerTaskCompleted;
        }

        _scanner.Scanned -= AddResources;
    }

    private void Claim(ResourceItem resourceItem, Worker worker)
    {
        resourceItem.Disable(_putTarget);
        _resourceData.CollectResource(resourceItem);
        ChangedResourceAmount?.Invoke(_resourceData.CollectedResources);
    }

    private void AddResources(ResourceItem scannedResource)
    {
       if ( _resourceData.TryAddResource(scannedResource))
       {
           SendWorker();
       }
    }

    private void SendWorker()
    {
        ResourceItem resource = null;
        Worker worker = null;
        
        if (_resourceData.HasFreeResources)
        {
            if (_workersData.TryGetFreeWorker(out worker) && _resourceData.TryGetFreeResource(out resource))
            {
                worker.SendToResource(resource);
                _resourceData.ReserveResource(resource);
            }
            else
            {
                if (resource != null)
                    _resourceData.TryAddResource(resource);
                
                if (worker != null)
                    _workersData.AddFreeWorker(worker);
            }
        }
    }
}