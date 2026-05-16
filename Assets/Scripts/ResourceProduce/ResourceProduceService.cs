using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProduceService : MonoBehaviour
{
    [SerializeField] private List<ResourceCreator>  _resourceCreators = new List<ResourceCreator>();

    private List<ResourceItem> _resourceItems = new List<ResourceItem>();
    
    public event Action ProduceCompleted;


    private void OnEnable()
    {
        foreach (ResourceCreator creator in _resourceCreators)
        {
            creator.ProduceCompleted += ResourceProduced;
        }
    }

    private void OnDisable()
    {
        foreach (ResourceCreator creator in _resourceCreators)
        {
            creator.ProduceCompleted -= ResourceProduced;
        }
    }

    public bool TryGetNotReservedResource(out ResourceItem resourceItem)
    {
        resourceItem =  null;
        
        if (_resourceItems.Count == 0)
            return false;

        for (int i = 0; i < _resourceItems.Count; i++)
        {
            resourceItem = _resourceItems[i];
            
            if (resourceItem.IsReserved == false)
            {
                resourceItem.SetReserved();
                return true;
            }
        }
        
        return false;
    }

    public void RemoveResource(ResourceItem resourceItem)
    {
        if (_resourceItems.Contains(resourceItem))
            _resourceItems.Remove(resourceItem);
    }

    public void ResourceProduced(ResourceItem resourceItem)
    {
        _resourceItems.Add(resourceItem);
        ProduceCompleted?.Invoke();
    }
}