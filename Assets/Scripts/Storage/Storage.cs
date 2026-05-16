using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Worker> _workers;
    [SerializeField] private ResourceProduceService _resourceService;
    [SerializeField] private Transform _inputZone;
    [SerializeField] private Transform _putTarget;
    [SerializeField] private StorageInfo _storageInfo;
    
    private List<ResourceItem> _collectedResources =  new List<ResourceItem>(); 
    
    public Transform InputZone => _inputZone;
    public Transform PutTarget => _putTarget;

    private void OnEnable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourceKeeped += WorkerKeepResource;
            worker.WorkCompleted += WorkCompleted;
        }
    }

    private void Start()
    {
        foreach (var worker in _workers)
        {
            worker.Initialize(this);
        }
    }

    private void OnDisable()
    {
        foreach (var worker in _workers)
        {
            worker.ResourceKeeped -= WorkerKeepResource;
            worker.WorkCompleted -= WorkCompleted;
        }
    }

    public void Claim(ResourceItem resourceItem)
    {
        resourceItem.Disable(_putTarget);
        _collectedResources.Add(resourceItem);
        _storageInfo.UpdateResourceAmount(_collectedResources.Count);
    }

    public void ResourceCreated()
    {
        SendWorker();
    }

    private void SendWorker()
    {
        for (int i = 0; i < _workers.Count; i++)
        {
            if (_workers[i].IsBusy ==  false)
            {
                Worker worker = _workers[i];
               
                if (_resourceService.TryGetNotReservedResource(out ResourceItem resourceItem) == false)
                    return;
        
                worker.SendToResource(resourceItem);
            }
        }
    }
    
    private void WorkCompleted()
    {
        SendWorker();
    }

    private void WorkerKeepResource(ResourceItem resourceItem)
    {
        _resourceService.RemoveResource(resourceItem);
    }
}