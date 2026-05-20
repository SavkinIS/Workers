using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Spawner
{
    public abstract class SpawnerBase<T> : MonoBehaviour where T : SpawnableObject 
    {
        protected ObjectPool<T> Pool;
        private readonly int _poolCapacity = 50;
    
        protected void CreatePool()
        {
            Pool = new ObjectPool<T>(
                createFunc: InstantiateSpawnableObject,
                actionOnGet: OnGetNextSpawnableObject,
                actionOnRelease: Release,
                defaultCapacity: _poolCapacity,
                maxSize: 100);
        }

        protected abstract void ReleasedToPool(T spawnableObject);
        protected abstract void Release(T spawnableObject);
        protected abstract void OnGetNextSpawnableObject(T spawnableObject);
        protected abstract T InstantiateSpawnableObject();
    
    }
}