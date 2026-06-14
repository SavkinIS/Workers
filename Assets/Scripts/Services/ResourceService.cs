using System.Collections.Generic;
using System.Linq;

public class ResourceService
{
    private readonly Dictionary<ResourceItem, List<Storage>> _resourceOwners =
        new Dictionary<ResourceItem, List<Storage>>();

    private readonly List<ResourceItem> _reservedResources =
        new List<ResourceItem>();

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

    public int GetAvailableResourcesCount(Storage storage)
    {
        return _resourceOwners.Count(pair =>
            pair.Key != null &&
            pair.Value.Contains(storage) &&
            _reservedResources.Contains(pair.Key) == false);
    }
}
