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
        Pool.Get(out ResourceItem item);
        return item;
    }

    protected override void ReleasedToPool(ResourceItem spawnableObject)
    {
        Pool.Release(spawnableObject);
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