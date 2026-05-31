using System;
using System.Collections.Generic;

public class ResourceService
{
    private Queue<ResourceItem> _freeResources = new Queue<ResourceItem>();
    private List<ResourceItem> _reservedResources = new List<ResourceItem>();
    private int _collectedResources = 0;

    public event Action<int> ChangedResourceAmount;
    
    public int CollectedResources => _collectedResources;
    public bool HasFreeResources => _freeResources.Count > 0;
    public int FreeResources => _freeResources.Count;


    public bool TryGetFreeResource(out ResourceItem resourceItem)
    {
        resourceItem = null;

        if (_freeResources.Count > 0)
        {
            while (resourceItem == null &&  _freeResources.Count > 0)
            {
                resourceItem = _freeResources.Dequeue();
            }

            if  (resourceItem == null)
                return false;
            
            if (_reservedResources.Contains(resourceItem) == false)
            {
                _reservedResources.Add(resourceItem);
            }
                
            return true;
        }

        return false;
    }

    public void CollectResource(ResourceItem resourceItem)
    {
        if (_reservedResources.Contains(resourceItem))
        {
            _reservedResources.Remove(resourceItem);
            _collectedResources++;
            ChangedResourceAmount?.Invoke(_collectedResources);
        }
    }

    public bool TryAddResource(ResourceItem resourceItem)
    {
        if (_freeResources.Contains(resourceItem) == false && _reservedResources.Contains(resourceItem) == false)
        {
            _freeResources.Enqueue(resourceItem);
            return true;
        }

        return false;
    }

    public bool TrySpendResource(int workerPrice)
    {
        if (_collectedResources >= workerPrice)
        {
            _collectedResources -= workerPrice;
            ChangedResourceAmount?.Invoke(_collectedResources);
            return true;
        }
        
        return false;
    }
}