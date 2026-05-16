using Spawner;
using UnityEngine;

public class ResourceSpawner : SpawnerBase<ResourceItem>
{
    [SerializeField] private ResourceItem _itemPrefab;

    private void Awake()
    {
        CreatePool();
    }

    public ResourceItem Spawn()
    {
        _pool.Get(out ResourceItem item);
        _activeObjects.Add(item);
        return item;
    }

    protected override void ReleasedToPool(ResourceItem spawnableObject)
    {
        _pool.Release(spawnableObject);
        _activeObjects.Remove(spawnableObject);
    }

    protected override void Release(ResourceItem spawnableObject)
    {
        spawnableObject.gameObject.SetActive(false);
        spawnableObject.Disabled -= ReleasedToPool;
    }

    protected override void OnGetNextSpawnableObject(ResourceItem spawnableObject)
    {
        spawnableObject.Disabled += ReleasedToPool;
        spawnableObject.gameObject.SetActive(true);
    }

    protected override ResourceItem InstantiateSpawnableObject()
    {
        ResourceItem item = Instantiate(_itemPrefab, transform);
        item.Initialize();
        return item;
    }
}