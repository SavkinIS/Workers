using System.Collections.Generic;

public class ResourceData
{
    private Queue<ResourceItem> _freeResources = new Queue<ResourceItem>();
    private List<ResourceItem> _reservedResources = new List<ResourceItem>();
    private int _collectedResources = 0;

    public int CollectedResources => _collectedResources;
    public bool HasFreeResources => _freeResources.Count > 0;

    public bool TryGetFreeResource(out ResourceItem resourceItem)
    {
        resourceItem = null;

        if (_freeResources.Count > 0)
        {
            resourceItem = _freeResources.Dequeue();

            if (resourceItem != null)
            {
                return true;
            }
        }

        return false;
    }

    public void ReserveResource(ResourceItem resourceItem)
    {
        if (_reservedResources.Contains(resourceItem) == false)
        {
            _reservedResources.Add(resourceItem);
        }
    }

    public void CollectResource(ResourceItem resourceItem)
    {
        if (_reservedResources.Contains(resourceItem))
        {
            _reservedResources.Remove(resourceItem);
            _collectedResources++;
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
}