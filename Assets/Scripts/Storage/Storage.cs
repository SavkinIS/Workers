using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Storage : MonoBehaviour
{
    private const int MinimalWorkersCount = 1;
    
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Transform _workerSpawnPoint;
    [SerializeField] private Flag _flag;
    [SerializeField] private ColorChanger _colorChanger;

    private ResourceService _resourceService;
    private WorkersService _workersService;
    private bool _newStorageGeneration = false;
    private WorkerSpawner _workerSpawner;
    private int _workerPrice;
    private int _storagePrice;
    private Color _storageColor;
    private Action<int> _changedResourceAmount;
    private Action<Vector3, Worker> _newStoragePositionReached;
    
    public Flag Flag => _flag;
    public bool CanBuildNext => _workers.Count > MinimalWorkersCount;

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
        _flag.Initialize();
        _workersService = new WorkersService(_workers);

        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget, _flag.transform, NewStoragePositionReached);
            worker.SetColor(_storageColor);
        }

        ResourceChangedSubscribe(ResourceChanged);
        _changedResourceAmount?.Invoke(_resourceService.CollectedResources(this));
    }

    private void NewStoragePositionReached(Worker worker)
    {
        var position = _flag.transform.position;
        _flag.ResetPosition();
        RemoveWorker(worker);
        worker.DropState();
        _newStoragePositionReached?.Invoke(position,worker);
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourcePutted -= Claim;
            worker.WorkCompleted -= WorkerTaskCompleted;
        }

        _scanner.Scanned -= AddResources;
        
        if (_resourceService != null)
            _resourceService.ChangedResourceAmount -= CallChangeResource;
    }

    private void OnDestroy()
    {
        _newStoragePositionReached = null;
    }

    public void ResourceChangedSubscribe(Action<int> updateResourceAmount)
    {
        _changedResourceAmount += updateResourceAmount;
    }

    public void Initialize(WorkerSpawner workerSpawner, int workerPrice, int storagePrice,
        Action<Vector3, Worker> newStoragePositionReached, ResourceService resourceService, Worker worker = null)
    {
        _workerSpawner = workerSpawner;
        _workerPrice = workerPrice;
        _storagePrice = storagePrice;
        _newStoragePositionReached += newStoragePositionReached;
        _resourceService = resourceService;
        
        _resourceService.RegisterStorage(this);
        _resourceService.ChangedResourceAmount += CallChangeResource;
        
        _storageColor = Random.ColorHSV();
        _storageColor.a = 1f;
        
        _colorChanger.SetColor(_storageColor);
        
        if (worker != null)
        {
            _workers.Add(worker);
            WorkerSetting(worker);
        }
    }
    
    public void EnableCollectNewBase()
    {
        _newStorageGeneration = true;

        if (_resourceService.CollectedResources(this) >= _storagePrice)
        {
            SendWorkerToNewStorage();
        }
    }
    
    private void WorkerSetting(Worker worker)
    {
        worker.ResourcePutted += Claim;
        worker.WorkCompleted += WorkerTaskCompleted;
        worker.SetColor(_storageColor);
    }

    private void ResourceChanged(int resources)
    {
        if (_newStorageGeneration)
        {
            if (_resourceService.CollectedResources(this) >= _storagePrice)
            {
                SendWorkerToNewStorage();
            }
        }
        else
        {
            CreateWorker();
        }
    }

    private void CreateWorker()
    {
        if (_resourceService.TrySpendResource(this,_workerPrice))
        {
            Worker worker = _workerSpawner.Spawn();
            _workers.Add(worker);
            worker.Initialize(_inputZone, _putTarget, _flag.transform, NewStoragePositionReached);
            worker.transform.position = _workerSpawnPoint.position;
            WorkerSetting(worker);
            _workersService.AddFreeWorker(worker);

            SendWorker();
        }
    }

    private void CallChangeResource(Storage storage, int amount)
    {
        if (storage.Equals(this))
            _changedResourceAmount?.Invoke(amount);
    }

    private void Claim(ResourceItem resourceItem, Worker worker)
    {
        resourceItem.Disable(_putTarget);
        _resourceService.CollectResource(this, resourceItem);
    }

    private void AddResources(ResourceItem scannedResource)
    {
       if ( _resourceService.TryAddResource(this, scannedResource))
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
        if (_newStorageGeneration && _resourceService.CollectedResources(this) >= _storagePrice && _workersService.HasFreeWorkers )
        {
            SendWorkerToNewStorage();
        }
        else
        {
            Worker worker = null;
            ResourceItem resource = null;

            if (_resourceService.GetAvailableResourcesCount(this) > 0 && _workersService.HasFreeWorkers)
            {
                worker = _workersService.GetFreeWorker();

                if (worker != null && _resourceService.TryGetFreeResource(this, out resource))
                {
                    worker.SendToResource(resource);
                }
            }
        }  
      
    }

    private void SendWorkerToNewStorage()
    {
        var worker = _workersService.GetFreeWorker();

        if (worker != null &&  _resourceService.TrySpendResource(this, _storagePrice))
        {
            worker.SendToNewStorage(_flag);
            _newStorageGeneration = false;
        }
    }

    private void RemoveWorker(Worker worker)
    {
        _workers.Remove(worker);
        worker.ResourcePutted -= Claim;
        worker.WorkCompleted -= WorkerTaskCompleted;
    }
}