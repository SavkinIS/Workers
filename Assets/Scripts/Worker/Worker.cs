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
    public event Action<ResourceItem> ResourcePutted;
    public event Action<Worker> WorkCompleted;
    
    public bool HasResource => _resource != null;
    public Transform HandPlace => _handPlace;
    public ResourceItem TargetResource { get;  private set;}
    public ResourceItem Resource => _resource;

    public void Initialize(Transform storageUnloadZone,Transform storagePutTarget, Transform flag, Action< Worker> newStoragePositionReached)
    {
        _stateMachine = new WorkerStateMachine(_mover, storageUnloadZone, storagePutTarget, this, flag, newStoragePositionReached);
    }

    public void SendToResource(ResourceItem resource)
    {
        TargetResource = resource;
        _stateMachine.SetState(typeof(MoveState));
    }
    
    public void KeepResource()
    {
       _resource = TargetResource;
       TargetResource = null;

       if (_resource == null)
           return;
       
       _resource.AttachTo(HandPlace);
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
        _stateMachine.SetState(typeof(MoveToBuildState));
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
}