using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Transform _workerSpawnPoint;

    private ResourceService _resourceService = new ResourceService();
    private WorkersService _workersService;
    
    private bool _newStorageGeneration = false;
    private WorkerSpawner _workerSpawner;
    private int _workerPrice;

    private Action<int> _changedResourceAmount;

    private void OnEnable()
    {
        _scanner.Scanned += AddResources;

        foreach (var worker in _workers)
        {
            worker.ResourcePutted += Claim;
            worker.WorkCompleted += WorkerTaskCompleted;
        }

        if (_resourceService != null)
            _resourceService.ChangedResourceAmount += CallChangeResource;
    }

    private void Start()
    {
        _workersService = new WorkersService(_workers);
        
        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget);
        }

        ResourceChangedSubscribe(CreateWorker);
        _changedResourceAmount?.Invoke(_resourceService.CollectedResources);
    }

    private void Update()
    {
        Debug.Log($"Ресурсов {_resourceService.FreeResources} \n Рабочих {_workersService.FreeWorkers}");
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourcePutted -= Claim;
            worker.WorkCompleted -= WorkerTaskCompleted;
        }

        _scanner.Scanned -= AddResources;
        _resourceService.ChangedResourceAmount -= CallChangeResource;
    }
    
    public void ResourceChangedSubscribe(Action<int> updateResourceAmount)
    {
        _changedResourceAmount += updateResourceAmount;
    }
    
    public void SetWorkerSpawner(WorkerSpawner workerSpawner, int workerPrice)
    {
        _workerSpawner =  workerSpawner;
        _workerPrice = workerPrice;
    }
    
    private void CreateWorker(int obj)
    {
        if (_newStorageGeneration)
            return;

        if (_resourceService.TrySpendResource(_workerPrice))
        {
            Worker worker = _workerSpawner.Spawn();
            _workers.Add(worker);
            worker.Initialize(_inputZone, _putTarget);
            worker.transform.position = _workerSpawnPoint.position;
            worker.ResourcePutted += Claim;
            worker.WorkCompleted += WorkerTaskCompleted;
            _workersService.AddFreeWorker(worker);
            
            int freeWorkers = _workersService.FreeWorkers;
            
            Debug.Log($"Куплен новый Рабочий \n Рабочих {_workersService.FreeWorkers}");
            
            SendWorker();
        }
    }
    
    private void CallChangeResource(int amount)
    {
        _changedResourceAmount?.Invoke(amount);
    }

    private void Claim(ResourceItem resourceItem, Worker worker)
    {
        resourceItem.Disable(_putTarget);
        _resourceService.CollectResource(resourceItem);
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