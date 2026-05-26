using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;

    private ResourceService _resourceService = new ResourceService();
    private WorkersService _workersService;

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
    
    private void Start()
    {
        _workersService = new WorkersService(_workers);
        
        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget);
        }

        ChangedResourceAmount?.Invoke(_resourceService.CollectedResources);
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
        _resourceService.CollectResource(resourceItem);
        ChangedResourceAmount?.Invoke(_resourceService.CollectedResources);
    }

    private void AddResources(ResourceItem scannedResource)
    {
       if ( _resourceService.TryAddResource(scannedResource))
       {
           SendWorker();
       }
    }

    private void WorkerTaskCompleted(Worker worker)
    {
        _workersService.AddFreeWorker(worker);
        
        SendWorker();
    }
    
    private void SendWorker()
    {
        ResourceItem resource = null;
        Worker worker = null;
        
        if (_resourceService.HasFreeResources && _workersService.HasFreeWorkers)
        {
            worker = _workersService.GetFreeWorker();
            
            if (worker != null && _resourceService.TryGetFreeResource(out resource))
            {
                worker.SendToResource(resource);
            }
        }
    }
}