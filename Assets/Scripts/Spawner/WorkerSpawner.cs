using Spawner;
using UnityEngine;

public class WorkerSpawner : SpawnerBase<Worker>
{
    [SerializeField] private Worker _itemPrefab;
    [SerializeField] private Transform _holder;

    private int _count = 0;

    private void Awake()
    {
        CreatePool();
    }

    public Worker Spawn()
    {
        Pool.Get(out Worker item);
        return item;
    }

    protected override void ReleasedToPool(Worker spawnableObject)
    {
        Pool.Release(spawnableObject);
    }

    protected override void Release(Worker spawnableObject)
    {
        spawnableObject.gameObject.SetActive(false);
    }

    protected override void OnGetNextSpawnableObject(Worker spawnableObject)
    {
        spawnableObject.gameObject.SetActive(true);
    }

    protected override Worker InstantiateSpawnableObject()
    {
        Worker item = Instantiate(_itemPrefab, _holder);
        item.name = $"{_itemPrefab.name}_{_count++}";
        return item;
    }
}