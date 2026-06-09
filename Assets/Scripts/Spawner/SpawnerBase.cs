using Spawner;
using UnityEngine;

public abstract class SpawnerBase<T> : MonoBehaviour where T : SpawnableObject
{
    protected int Count;
    protected abstract T InstantiateSpawnableObject();
}