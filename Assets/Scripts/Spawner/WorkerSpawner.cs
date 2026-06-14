using UnityEngine;

public class WorkerSpawner : SpawnerBase<Worker>
{
    [SerializeField] private Worker _itemPrefab;
    [SerializeField] private Transform _holder;
    [SerializeField] private Transform _workerSpawnPoint;

    public Worker Spawn()
    {
        Worker worker = InstantiateSpawnableObject();
        worker.transform.position = _workerSpawnPoint.position;

        return worker;
    }
    
    protected override Worker InstantiateSpawnableObject()
    {
        Worker item = Instantiate(_itemPrefab, _holder);
        item.name = $"{_itemPrefab.name}_{Count++}";
        return item;
    }
}