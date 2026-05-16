using System;
using UnityEngine;
using WorkerStates;

public class Worker : MonoBehaviour
{
    [SerializeField] private TargetMover _targetMover;
    [SerializeField] private Transform _handPlace;

    private WorkerStateMachine _stateMachine;
    
    private ResourceItem _resource;
    private Storage _storage;

    public bool HasResource => _resource != null;
    public Transform HandPlace => _handPlace;
    public bool IsBusy { get; private set; }
    public ResourceItem TargetResource { get;  private set;}
    public ResourceItem Resource => _resource;
    public event Action WorkCompleted;

    public event Action<ResourceItem> ResourceKeeped;

    public void Initialize(Storage storage)
    {
        _storage =  storage;
        _stateMachine = new WorkerStateMachine(_targetMover, _storage, this);
    }

    public void SendToResource(ResourceItem resource)
    {
        TargetResource = resource;
        IsBusy = true;
        _stateMachine.SetState(typeof(MoveState));
    }
    
    public void KeepResource()
    {
       _resource = TargetResource;
       TargetResource = null;

       if (_resource == null)
           return;
       
       ResourceKeeped?.Invoke(_resource);
       _resource.SetParent(HandPlace);
    }

    public void PutResource()
    {
        _resource = null;
        IsBusy = false;
        _stateMachine.SetState(typeof(IdleState));
    }

    public void CompleteWork()
    {
        WorkCompleted?.Invoke();
    }
    
}