using System;
using UnityEngine;

public class StorageService : MonoBehaviour
{
    [SerializeField] private Storage _storageStart;
    [SerializeField] private WorkerSpawner _workerSpawner;
    [SerializeField] private Storage _storagePrefab;
    [SerializeField] private int _workerPrice = 3;

    private void Awake()
    {
        _storageStart.SetWorkerSpawner(_workerSpawner, _workerPrice);
    }
}