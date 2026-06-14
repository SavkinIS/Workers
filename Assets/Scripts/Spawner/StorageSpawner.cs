using UnityEngine;

public class StorageSpawner : SpawnerBase<Storage>
{
    [SerializeField] private Storage _itemPrefab;
    [SerializeField] private Transform _holder;

    public Storage Spawn(Vector3 newPosition)
    {
        Storage storage = InstantiateSpawnableObject();
        storage.transform.position = newPosition;
        return storage;
    }
    
    protected override Storage InstantiateSpawnableObject()
    {
        Storage item = Instantiate(_itemPrefab, _holder);
        item.name = $"{_itemPrefab.name}_{Count++}";
        return item;
    }
}