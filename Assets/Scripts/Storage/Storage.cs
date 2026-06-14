using System;
using Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

public class Storage : SpawnableObject
{
    private const int MinimalWorkersCount = 1;

    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Flag _flag;
    [SerializeField] private ColorChanger _colorChanger;
    [SerializeField] private WorkersManager _workersManager;

    private ResourceService _resourceService;
    private bool _isBuildingNewStorage = false;

    private int _workerPrice;
    private int _storagePrice;
    private Color _storageColor;
    private Action<Vector3, Worker> _newStoragePositionReached;
    private Wallet _wallet = new Wallet();

    public event Action<int> СhangedResourceAmount;

    public Flag Flag => _flag;
    public bool CanBuildNext => _workersManager.Count > MinimalWorkersCount;

    private void OnEnable()
    {
        _scanner.Scanned += AddResources;
        _workersManager.ResourcePutted += Claim;
        _workersManager.WorkCompleted += WorkerTaskCompleted;
        _wallet.СhangedResourceAmount += CallChangeResource;

        СhangedResourceAmount += ResourceChanged;
    }

    private void Start()
    {
        _flag.Initialize();

        СhangedResourceAmount?.Invoke(_wallet.Amount);
    }

    private void OnDisable()
    {
        _workersManager.ResourcePutted -= Claim;
        _workersManager.WorkCompleted -= WorkerTaskCompleted;
        _scanner.Scanned -= AddResources;
        _wallet.СhangedResourceAmount -= CallChangeResource;

        СhangedResourceAmount -= ResourceChanged;
    }

    private void OnDestroy()
    {
        _newStoragePositionReached = null;
    }

    public void Initialize(int workerPrice, int storagePrice,
        Action<Vector3, Worker> newStoragePositionReached, ResourceService resourceService, Worker worker = null)
    {
        _workerPrice = workerPrice;
        _storagePrice = storagePrice;
        _newStoragePositionReached += newStoragePositionReached;
        _resourceService = resourceService;
        _storageColor = Random.ColorHSV();
        _storageColor.a = 1f;

        _colorChanger.SetColor(_storageColor);
        _workersManager.Initialize(_inputZone, _putTarget, _flag.transform, NewStoragePositionReached, _storageColor,
            worker);
    }

    public void EnableNewStorageConstruction()
    {
        _isBuildingNewStorage = true;

        if (_wallet.Amount >= _storagePrice)
        {
            SendWorkerToNewStorage();
        }
    }

    private void ResourceChanged(int resources)
    {
        if (_isBuildingNewStorage)
        {
            if (_wallet.Amount >= _storagePrice)
            {
                SendWorkerToNewStorage();
            }

            return;
        }

        CreateWorker();
    }

    private void CreateWorker()
    {
        if (_wallet.TrySpendResource(_workerPrice))
        {
            _workersManager.CreateWorker();

            SendWorker();
        }
    }

    private void CallChangeResource(int amount)
    {
        СhangedResourceAmount?.Invoke(amount);
    }

    private void Claim(ResourceItem resourceItem)
    {
        resourceItem.Disable(_putTarget);
        _wallet.CollectResource();
    }

    private void AddResources(ResourceItem scannedResource)
    {
        if (_resourceService.TryAddResource(this, scannedResource))
        {
            SendWorker();
        }
    }

    private void WorkerTaskCompleted(Worker worker)
    {
        _workersManager.AddFreeWorker(worker);
        SendWorker();
    }

    private void SendWorker()
    {
        if (_isBuildingNewStorage && _wallet.Amount >= _storagePrice && _workersManager.HasFreeWorkers)
        {
            SendWorkerToNewStorage();
        }
        else
        {
            if (_resourceService.GetAvailableResourcesCount(this) > 0 && _workersManager.HasFreeWorkers)
            {
                if (_resourceService.TryGetFreeResource(this, out ResourceItem resource))
                {
                    _workersManager.SendWorkerToResource(resource);
                }
            }
        }
    }

    private void SendWorkerToNewStorage()
    {
        if (_workersManager.HasFreeWorkers && _wallet.TrySpendResource(_storagePrice))
        {
            _workersManager.SendWorkerToNewStorage();
            _isBuildingNewStorage = false;
        }
    }

    private void NewStoragePositionReached(Worker worker)
    {
        var position = _flag.transform.position;
        _flag.ResetPosition();
        _workersManager.RemoveWorker(worker);
        _newStoragePositionReached?.Invoke(position, worker);
    }
}