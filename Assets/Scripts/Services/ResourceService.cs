using System;
using System.Collections.Generic;
using System.Linq;

public class ResourceService
{
    private readonly Dictionary<ResourceItem, List<Storage>> _resourceOwners =
        new Dictionary<ResourceItem, List<Storage>>();

    private readonly List<ResourceItem> _reservedResources =
        new List<ResourceItem>();

    private readonly Dictionary<Storage, int> _collectedResources = new Dictionary<Storage, int>();

    public event Action<Storage, int> ChangedResourceAmount;

    public void RegisterStorage(Storage storage)
    {
        _collectedResources[storage] = 0;
    }
    
    public int CollectedResources(Storage storage) => _collectedResources[storage];

    public bool TryAddResource(Storage storage, ResourceItem resource)
    {
        if (resource == null || storage == null)
            return false;

        if (_reservedResources.Contains(resource))
            return false;

        if (_resourceOwners.TryGetValue(resource, out var owners) == false)
        {
            owners = new List<Storage>();
            _resourceOwners.Add(resource, owners);
        }

        if (owners.Contains(storage))
            return false;

        owners.Add(storage);
        return true;
    }

    public bool TryGetFreeResource(Storage storage, out ResourceItem resource)
    {
        resource = null;

        foreach (var pair in _resourceOwners)
        {
            ResourceItem candidate = pair.Key;
            List<Storage> owners = pair.Value;

            if (candidate == null)
                continue;

            if (owners.Contains(storage) == false)
                continue;

            if (_reservedResources.Contains(candidate))
                continue;
            
            _reservedResources.Add(candidate);
            resource = candidate;
            _resourceOwners.Remove(candidate);
            return true;
        }

        return false;
    }

    public void CollectResource(Storage storage, ResourceItem resource)
    {
        if (resource == null)
            return;

        _reservedResources.Remove(resource);

        _collectedResources[storage]++;
        ChangedResourceAmount?.Invoke(storage, _collectedResources[storage]);
    }

    public bool TrySpendResource(Storage storage, int amount)
    {
        if (_collectedResources[storage] < amount)
            return false;

        _collectedResources[storage] -= amount;
        ChangedResourceAmount?.Invoke(storage, _collectedResources[storage]);

        return true;
    }

    public int GetAvailableResourcesCount(Storage storage)
    {
        return _resourceOwners.Count(pair =>
            pair.Key != null &&
            pair.Value.Contains(storage) &&
            _reservedResources.Contains(pair.Key) == false);
    }
}
