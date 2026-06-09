using UnityEngine;

public class WorkerSpawner : SpawnerBase<Worker>
{
    [SerializeField] private Worker _itemPrefab;
    [SerializeField] private Transform _holder;

    public Worker Spawn()
    {
        return InstantiateSpawnableObject();
    }
    
    protected override Worker InstantiateSpawnableObject()
    {
        Worker item = Instantiate(_itemPrefab, _holder);
        item.name = $"{_itemPrefab.name}_{Count++}";
        return item;
    }
}