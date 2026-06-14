using UnityEngine;

public interface IWorkerTask
{
    Transform Target { get;}
    void ExecuteCompletion();
}