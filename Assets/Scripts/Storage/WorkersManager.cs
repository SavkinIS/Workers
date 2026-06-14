using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers = new List<Worker>();
    [SerializeField] private WorkerSpawner _workerSpawner;

    private Transform _inputZone;
    private Transform _putTarget;
    private Action<Worker> _newStoragePositionReached;
    private Transform _flag;
    private Color _storageColor;
    private WorkersService _workersService;
    private bool _isInitialize;

    public event Action<ResourceItem> ResourcePutted;
    public event Action<Worker> WorkCompleted;
    public int Count => _workers.Count;
    public bool HasFreeWorkers => _workersService.HasFreeWorkers;

    private void OnEnable()
    {
        if (_isInitialize)
        {
            foreach (var worker in _workers)
            {
                worker.ResourcePutted += ResourcePuttedToStorage;
                worker.WorkCompleted += WorkerTaskCompleted;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourcePutted -= ResourcePuttedToStorage;
            worker.WorkCompleted -= WorkerTaskCompleted;
        }
    }

    public void Initialize(Transform inputZone, Transform putTarget, Transform flag,
        Action<Worker> newStoragePositionReached, Color storageColor, Worker startWorker = null)
    {
        _inputZone = inputZone;
        _putTarget = putTarget;
        _storageColor = storageColor;
        _flag = flag;
        _newStoragePositionReached = newStoragePositionReached;

        if (startWorker  != null)
            _workers.Add(startWorker);
        
        foreach (var worker in _workers)
        {
            worker.Initialize(_inputZone, _putTarget, _flag.transform, _newStoragePositionReached);
            worker.SetColor(_storageColor);
            WorkerSetting(worker);
        }
        
        _workersService = new WorkersService(_workers);
        _isInitialize = true;
    }

    public void RemoveWorker(Worker worker)
    {
        _workers.Remove(worker);
        worker.DropState();
        worker.ResourcePutted -= ResourcePuttedToStorage;
        worker.WorkCompleted -= WorkerTaskCompleted;
    }

    public void CreateWorker()
    {
        Worker worker = _workerSpawner.Spawn();
        _workers.Add(worker);
        worker.Initialize(_inputZone, _putTarget, _flag, _newStoragePositionReached);
        WorkerSetting(worker);
        _workersService.AddFreeWorker(worker);
    }

    public void AddFreeWorker(Worker worker)
    {
        _workersService.AddFreeWorker(worker);
    }
    
    public void SendWorkerToResource(ResourceItem resource)
    {
        if (resource == null)
            return;
        
        var worker = _workersService.GetFreeWorker();
        worker.SendToResource(resource);
    }

    public void SendWorkerToNewStorage()
    {
        var worker = _workersService.GetFreeWorker();
        worker.SendToNewStorage();
    }
    
    private void WorkerSetting(Worker worker)
    {
        worker.ResourcePutted += ResourcePuttedToStorage;
        worker.WorkCompleted += WorkerTaskCompleted;
        worker.SetColor(_storageColor);
    }
    
    private void WorkerTaskCompleted(Worker obj)
    {
        WorkCompleted?.Invoke(obj);
    }

    private void ResourcePuttedToStorage(ResourceItem resourceItem)
    {
        ResourcePutted?.Invoke(resourceItem);
    }
}