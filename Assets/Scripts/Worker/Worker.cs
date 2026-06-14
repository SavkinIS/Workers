using System;
using Spawner;
using UnityEngine;
using WorkerStates;

public class Worker : SpawnableObject
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _handPlace;
    [SerializeField] private ColorChanger _colorChanger;
    
    private WorkerStateMachine _stateMachine;
    private ResourceItem _resource;
    private IWorkerTask _task;
    private Transform _storageFlag;
    private Transform _storageUnloadZone;
    private Action<Worker> _newStoragePositionReached;

    public event Action<ResourceItem> ResourcePutted;
    public event Action<Worker> WorkCompleted;
    
    public bool HasResource => _resource != null;
    public Transform HandPlace => _handPlace;
    public ResourceItem TargetResource { get;  private set;}
    public ResourceItem Resource => _resource;
    public IWorkerTask Task => _task;

    public void Initialize(Transform storageUnloadZone,Transform storagePutTarget, Transform flag, Action< Worker> newStoragePositionReached)
    {
        _storageFlag = flag;
        _storageUnloadZone = storageUnloadZone;
        _newStoragePositionReached = newStoragePositionReached;
        _stateMachine = new WorkerStateMachine(_mover, storagePutTarget, this);
    }

    public void SendToResource(ResourceItem resource)
    {
        TargetResource  = resource;
        _task = new MoveTask(resource.transform, ExecuteKeepState);
        _stateMachine.SetState(typeof(MoveState));
    }

    public void KeepResource()
    {
       _resource = TargetResource;
       TargetResource = null;

       if (_resource == null)
           return;
       
       _resource.AttachTo(HandPlace);
       SendToStorage();
    }

    public void PutResource()
    {
        ResourcePutted?.Invoke(_resource);
        _resource = null;
        _stateMachine.SetState(typeof(IdleState));
        WorkCompleted?.Invoke(this);
    }

    public void SendToNewStorage()
    {
        _task = new MoveTask(_storageFlag, NewStorageReached);
        _stateMachine.SetState(typeof(MoveState));
    }

    public void DropState()
    {
        _stateMachine.Dispose();
        _stateMachine =  null;
    }

    public void SetColor(Color color)
    {
        _colorChanger.SetColor(color);
    }
    
    private void ExecuteKeepState()
    {
        _stateMachine.SetState((typeof(KeepState)));
    }

    private void SendToStorage()
    {
        _task = new MoveTask(_storageUnloadZone, ExecutePutState);
        _stateMachine.SetState(typeof(MoveState));
    }

    private void ExecutePutState()
    {
        _stateMachine.SetState((typeof(PutState)));
    }

    private void NewStorageReached()
    {
        _newStoragePositionReached?.Invoke(this);
    }
}