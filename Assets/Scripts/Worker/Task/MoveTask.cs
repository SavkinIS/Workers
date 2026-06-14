using System;
using UnityEngine;

public class MoveTask : IWorkerTask
{
    private readonly Action _completedTask;

    public MoveTask(Transform storage, Action completeTask)
    {
        Target = storage;
        _completedTask = completeTask;
    }
    
    public Transform Target { get; private set; }
    
    public void ExecuteCompletion()
    {
        _completedTask?.Invoke();
    }
}